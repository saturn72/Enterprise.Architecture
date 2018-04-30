using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Programmer.WebServer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Programmer.Test.Framework
{
    public abstract class IntegrationTestBase:IDisposable
    {
        protected static readonly Uri BaseUri = new Uri("http://localhost:5001/");
        private IModel _channel;
        private IConnection _connection;
        protected readonly HttpClient Client;
        protected readonly IConfigurationProvider Configuration;


        protected IntegrationTestBase()
        {
            var path = PlatformServices.Default.Application.ApplicationBasePath;
            var serverDirectory = Path.GetFullPath(Path.Combine(path, @"../../../../../Programmer.WebServer"));
            if (!Directory.Exists(serverDirectory))
                throw new DirectoryNotFoundException("Failed to find server root dir in path: " + serverDirectory);

            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(serverDirectory)
                .AddJsonFile("appsettings.Test.json")
                .Build();

            var builder = new WebHostBuilder()
                .UseContentRoot(serverDirectory)
                .UseStartup<Startup>()
                .UseUrls(BaseUri.AbsoluteUri)
                .UseConfiguration(configurationRoot);

            Configuration = configurationRoot.Providers.FirstOrDefault();

            var server = new TestServer(builder);
            Client = server.CreateClient();
            Client.BaseAddress = BaseUri;


            SetupRabbitMq();
        }

        private void SetupRabbitMq()
        {
            var queueName = "auto-programmer-command-queue";
            var exchangeName = "programmer-exchange";
            var exchangeType = "fanout";
            var rabbitMqHostName = "localhost";
            var rabbitMqUsername = "guest";
            var rabbitMqPassword = "guest";
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqHostName,
                UserName = rabbitMqUsername,
                Password = rabbitMqPassword
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchangeName, exchangeType);

            _channel.QueueDeclare(queueName);
            _channel.QueueBind(queueName,
                exchangeName, "CrudIntegrationEvent`1");

            var consumer = new EventingBasicConsumer(_channel);

            var dirName = "integration-test-output";
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);

            var fileName = dirName+ "/output_{0}.log";
            
            var index = 0;
            consumer.Received += (model, ea) => {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                File.WriteAllLines(string.Format(fileName, index++), new[] { message });
            };
            _channel.BasicConsume(queueName,
                true,
                consumer);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
            Client?.Dispose();
        }
    }
}