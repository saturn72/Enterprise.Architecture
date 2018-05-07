using System;
using System.Collections.Generic;
using System.Text;
using EventBus;
using Microsoft.Extensions.Configuration;
using Programmer.Common.Services.Command;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceStack.Text;

namespace Programmer.Test.Framework.Q
{
    public class EventQueueManager : IDisposable
    {
        private IModel _channel;
        private IConnection _connection;

        private static ICollection<string> _incomingEvents;
        private RabbitMqConfig _rabbitMqConfig;
        public static IEnumerable<string> IncomingEvents => _incomingEvents ?? (_incomingEvents = new List<string>());

        public void Setup(IConfigurationProvider configuration)
        {
            _rabbitMqConfig = GetRabbitMqConfig(configuration);

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqConfig.HostName,
                UserName = _rabbitMqConfig.Username,
                Password = _rabbitMqConfig.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            //register to programmer's outgoing exchange
            _channel.ExchangeDeclare(_rabbitMqConfig.ExchangeName, _rabbitMqConfig.ExchangeType);
            _channel.QueueDeclare(
                queue: _rabbitMqConfig.OutgoingQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.QueueBind(
                queue: _rabbitMqConfig.OutgoingQueueName, 
                exchange: _rabbitMqConfig.ExchangeName, 
                routingKey: typeof(IntegrationEvent<CommandRequest>).ToCSharpName());

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) => Handle(ea.Body);
            _channel.BasicConsume(_rabbitMqConfig.IncomingQueueName, true, consumer);
        }

        private RabbitMqConfig GetRabbitMqConfig(IConfigurationProvider configuration)
        {
            configuration.TryGet("RabbitMqHost", out var rabbitMqHostName);
            configuration.TryGet("RabbitMqUsername", out var rabbitMqUsername);
            configuration.TryGet("RabbitMqPassword", out var rabbitMqPassword);
            configuration.TryGet("ExhangeName", out var exchangeName);
            configuration.TryGet("ExchangeType", out var outgoingExchangeType);
            configuration.TryGet("outgoingQueueName", out var outgoingQueueName);
            configuration.TryGet("incomingQueueName", out var incomingQueueName);

            return new RabbitMqConfig
            {
                HostName = rabbitMqHostName,
                Username = rabbitMqUsername,
                Password = rabbitMqPassword,
                ExchangeName = exchangeName,
                ExchangeType = outgoingExchangeType,
                OutgoingQueueName = outgoingQueueName,
                IncomingQueueName = incomingQueueName
            };
        }

        public void Flush()
        {
            _incomingEvents?.Clear();
        }

        public void Handle(byte[] body)
        {
            var message = Encoding.UTF8.GetString(body);
            (IncomingEvents as ICollection<string>)?.Add(message);
        }

        public void Publish<TEvent>(TEvent @event)
        {
            var message = JsonSerializer.SerializeToString(@event);

            Publish(message, @event.ToCSharpName());
        }

        public void Publish(string message, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: _rabbitMqConfig.ExchangeName,
                routingKey: routingKey,
                basicProperties: null,
                body: body);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
