using Visma.Timelogger.EventBus.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.EventHandlers;
using Microsoft.Extensions.Logging;

namespace Visma.Timelogger.EventBus
{
    public static class EventBusServiceRegistration
    {
        public static IServiceCollection AddEventBusServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQSettings>(configuration.GetSection(RabbitMQSettings.Position));

            //Domain Bus
            var settings = configuration.GetSection(RabbitMQSettings.Position).Get<RabbitMQSettings>();
            if (settings is null)
            {
                throw new NullReferenceException("settings.Value.cannot be null");
            }
            services.AddTransient<IEventBus, RabbitMQBus>(sp =>
                    {
                        var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                        var logger = sp.GetRequiredService<ILogger<RabbitMQBus>>(); // Resolve ILogger<RabbitMQBus>

                        return new RabbitMQBus(settings, scopeFactory, logger);
                    });

            //EventHandlers
            services.AddTransient<ProjectCreatedEventHandler>();
            return services;
        }

    }
}