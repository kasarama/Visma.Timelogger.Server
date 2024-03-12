using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.Events;

namespace Visma.Timelogger.EventBus.RabbitMQ
{
    public class RabbitMQBus : IEventBus
    {
        public RabbitMQSettings _settings { get; }
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _evenTypes;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<RabbitMQBus> _logger;

        public RabbitMQBus(RabbitMQSettings settings,
                           IServiceScopeFactory serviceScopeFactory,
                           ILogger<RabbitMQBus> logger)
        {
            _settings = settings;
            _handlers = new Dictionary<string, List<Type>>();
            _evenTypes = new List<Type>();
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task PublishAsync<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateChannel();

            channel.ConfirmSelect();

            string eventName = @event.GetType().Name;
            channel.QueueDeclare(eventName, true, false, false, null);
            channel.BasicAcks += (sender, eventArgs) =>
            {
                _logger.LogInformation($"Message published with delivery tag: {eventArgs.DeliveryTag}");
            };
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync("", eventName, body: body);
        }


        public async Task SubscribeAsync<T, TH>() where T : Event where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_evenTypes.Contains(typeof(T)))
            {
                _evenTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(s => s == handlerType))
            {
                _logger.LogError($"Handler Type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType));
                throw new ArgumentException($"Handler Type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType));
            }

            _handlers[eventName].Add(handlerType);

            await StartBasicConsumer<T>();

        }

        private async Task StartBasicConsumer<T>() where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateChannel();

            var eventName = typeof(T).Name;
            channel.QueueDeclare(eventName, true, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            await channel.BasicConsumeAsync(eventName, false, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
                ((AsyncDefaultBasicConsumer)sender).Channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Something went wrong with Consumer_Received!");
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handlers[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetRequiredService(subscription);

                        if (handler == null)
                        {
                            continue;
                        }

                        var eventType = _evenTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                        await ((Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event })).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}
