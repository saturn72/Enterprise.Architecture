using System;
using System.Linq;
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
        public static void RegisterRabbitMqPublisher(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfig>(rmc =>
            {
                rmc.Host = configuration["RabbitMqHost"];
                rmc.Username = configuration["RabbitMqUsername"];
                rmc.Password = configuration["RabbitMqPassword"];
                rmc.ExhangeName = configuration["ExhangeName"];
                rmc.ExchangeType = configuration["ExchangeType"];
                rmc.OutgoingQueueName = configuration["outgoingQueueName"];
                rmc.IncomingQueueName = configuration["incomingQueueName"];

                var retryCount = 20;
                if (configuration["EventBusRetryCount"].HasValue())
                    int.TryParse(configuration["EventBusRetryCount"], out retryCount);
                rmc.MaxRetries = retryCount;
            });

            services.AddSingleton<IEventBus>(sp =>new RabbitMqEventBus.EventBus(sp.GetService<IRabbitMQPersistentConnection>(),
                    sp.GetService<ILogger<RabbitMqEventBus.EventBus>>(),
                    sp.GetService<ISubscriptionsManager>(),
                    sp.GetService<RabbitMqConfig>(),
                    sp));

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

                return new DefaultRabbitMQPersistentConnection(factory, logger, rmqConfig.MaxRetries);
            });
        }
    }
}