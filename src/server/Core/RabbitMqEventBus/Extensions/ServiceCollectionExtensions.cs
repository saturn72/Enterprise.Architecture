using System;
using System.Collections.Generic;
using EventBus;
using EventBus.Subscriptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMqEventBus;
using RabbitMqEventBus.Config;
using RabbitMQ.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterRabbitMq(this IServiceCollection services, IConfiguration configuration, IEnumerable<Type> integrationEventHandlersTypes)
        {
            ConfigureQueue(services, configuration);
            if (integrationEventHandlersTypes.IsNullOrEmpty())
                return;
            foreach (var handlerType in integrationEventHandlersTypes)
                services.AddTransient(handlerType);
        }

        private static void ConfigureQueue(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfig>(rmc =>
            {
                rmc.Host = configuration["RabbitMqHost"];
                rmc.Username = configuration["RabbitMqUsername"];
                rmc.Password = configuration["RabbitMqPassword"];
                rmc.BrokerName = configuration["BrokerName"];
                rmc.ExchangeType = configuration["ExchangeType"];
                rmc.QueueName = configuration["QueueName"];

                var retryCount = 20;
                if (configuration["EventBusRetryCount"].HasValue())
                    int.TryParse(configuration["EventBusRetryCount"], out retryCount);
                rmc.MaxRetries = retryCount;
            });

            services.AddSingleton<IEventBus, RabbitMqEventBus.EventBus>();
            services.AddSingleton<ISubscriptionsManager, InMemorySubscriptionsManager>();
            services.AddSingleton(sp => sp.GetService<IOptions<RabbitMqConfig>>().Value);
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var rmqConfig = sp.GetService<RabbitMqConfig>();

                var factory = new ConnectionFactory()
                {
                    HostName = rmqConfig.Host,
                    UserName = rmqConfig.Username,
                    Password = rmqConfig.Password,
                };
                if (!factory.HostName.HasValue()
                    || !factory.UserName.HasValue()
                    || !factory.Password.HasValue())
                    throw new ArgumentException("Cannot initiate RabbitMQ. Missing IP, Username or password");

                return new DefaultRabbitMQPersistentConnection(factory, logger, rmqConfig);
            });
        }
    }
}
