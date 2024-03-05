using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.Events;

namespace Visma.Timelogger.EventBus.RabbitMQ
{
    public class RabbitMQBus : IEventBus
    {
        public RabbitMQSettings _settings { get; }

        public RabbitMQBus(RabbitMQSettings settings, IServiceScopeFactory serviceScopeFactory)
        {
            _settings = settings;
        }

        public async Task PublishAsync<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateChannel();
            string eventName = @event.GetType().Name;
            channel.QueueDeclare(eventName, true, false, false, null);
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync("", eventName, body: body);
        }
    }
}
