using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMqEventBus.Config;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using EventBus;
using EventBus.Subscriptions;
using RabbitMQ.Client.Events;
using ServiceStack.Text;

namespace RabbitMqEventBus
{
    public class EventBus : IEventBus
    {
        #region Fields
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ISubscriptionsManager _subscriptionsManager;
        private readonly ILogger<EventBus> _logger;
        private readonly RabbitMqConfig _config;
        private readonly IServiceProvider _serviceProvider;
        private IModel _consumerChannel;

        #endregion

        #region ctor

        public EventBus(IRabbitMQPersistentConnection persistentConnection, ILogger<EventBus> logger,
            ISubscriptionsManager subscriptionsManager, RabbitMqConfig config, IServiceProvider serviceProvider)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _subscriptionsManager = subscriptionsManager ?? throw new ArgumentNullException(nameof(subscriptionsManager));
            _subscriptionsManager.OnEventRemoved += SubsManager_OnEventRemoved;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config;
            _serviceProvider = serviceProvider;
        }

        #endregion

        public void Publish(IntegrationEvent @event)
        {
            TryConnectIfDisconnected();
            var addToQueuePolicy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_config.MaxRetries,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) => _logger.LogWarning(ex.ToString()));

            using (var channel = _persistentConnection.CreateModel())
            {
                var eventName = _subscriptionsManager.GetEventKey(@event.GetType());

                channel.ExchangeDeclare(_config.ExhangeName, _config.ExchangeType);
                var message = JsonSerializer.SerializeToString(@event);
                var body = Encoding.UTF8.GetBytes(message);

                addToQueuePolicy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    channel.BasicPublish(exchange: _config.ExhangeName,
                                     routingKey: eventName,
                                     mandatory: true,
                                     basicProperties: properties,
                                     body: body);
                });
            }
        }

        public void Subscribe<TIntegrationEvent, TIntegrationEventHandler>()
            where TIntegrationEvent : IntegrationEvent
            where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent>
        {
            var eventName = _subscriptionsManager.GetEventKey<TIntegrationEvent>();
            DoInternalSubscription(eventName);
            _subscriptionsManager.AddSubscription<TIntegrationEvent, TIntegrationEventHandler>();
        }

        public void SubscribeDynamic<TDynamicIntegrationEventHandler>(string eventName) where TDynamicIntegrationEventHandler : IDynamicIntegrationEventHandler
        {
            DoInternalSubscription(eventName);
            _subscriptionsManager.AddDynamicSubscription<TDynamicIntegrationEventHandler>(eventName);
        }

        public void Unsubscribe<TIntegrationEvent, TIntegrationEventHandler>()
            where TIntegrationEvent : IntegrationEvent
            where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent>
        {
            _subscriptionsManager.RemoveSubscription<TIntegrationEvent, TIntegrationEventHandler>();
        }

        public void UnsubscribeDynamic<TDynamicIntegrationEventHandler>(string eventName) where TDynamicIntegrationEventHandler : IDynamicIntegrationEventHandler
        {
            _subscriptionsManager.RemoveDynamicSubscription<TDynamicIntegrationEventHandler>(eventName);
        }

        #region Utilities

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueUnbind(queue: _config.IncomingQueueName,
                    exchange: _config.ExhangeName,
                    routingKey: eventName);

                if (_subscriptionsManager.IsEmpty)
                    _consumerChannel.Close();
            }
        }

        private IModel CreateConsumerChannel()
        {
            var channel = _persistentConnection.CreateModel();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body);

                await ProcessEvent(eventName, message);

                channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: _config.OutgoingQueueName,
                autoAck: false,
                consumer: consumer);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subscriptionsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptionInfos = _subscriptionsManager.GetHandlersForEvent(eventName);
                foreach (var subInfo in subscriptionInfos)
                {
                    var handler = _serviceProvider.GetService(subInfo.HandlerServiceType);

                    if (subInfo.IsDynamic)
                    {
                        dynamic eventData = JsonObject.Parse(message);

                        await ( handler  as IDynamicIntegrationEventHandler).Handle(eventData);
                    }
                    else
                    {
                        var eventType = _subscriptionsManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonSerializer.DeserializeFromString(message, eventType);
                        var intHandler =  handler as IIntegrationEventHandler;

                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(intHandler, new[] { integrationEvent });
                    }
                }
            }
        }

        private void TryConnectIfDisconnected()
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subscriptionsManager.HasSubscriptionsForEvent(eventName);
            if (containsKey)
                return;
            TryConnectIfDisconnected();
            DeclareExchangeAndBindQueue(eventName);

            _consumerChannel = CreateConsumerChannel();
        }

        private void DeclareExchangeAndBindQueue(string eventName)
        {
            using (var channel = _persistentConnection.CreateModel())
            {
                channel.ExchangeDeclare(_config.ExhangeName, _config.ExchangeType);

                channel.QueueDeclare(queue: _config.IncomingQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.QueueBind(queue: _config.IncomingQueueName,
                    exchange: _config.ExhangeName,
                    routingKey: eventName);
            }
        }

        #endregion
    }
}
