using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Programmer.Test.Framework.Q;
using Programmer.WebServer;

namespace Programmer.Test.Framework
{
    public abstract class IntegrationTestBase : IDisposable
    {
        protected readonly HttpClient HttpClient;
        protected readonly WebSocket WebSocket;
        protected readonly IConfigurationProvider Configuration;
        protected readonly EventQueueManager EventQueueManager;
        protected IEnumerable<byte> Buffer => _buffer.Array;

        private readonly ArraySegment<byte> _buffer;

        protected IntegrationTestBase()
        {
            var path = PlatformServices.Default.Application.ApplicationBasePath;
            var serverDirectory = Path.GetFullPath(Path.Combine(path, @"../../../../../Application/Programmer.WebServer"));
            if (!Directory.Exists(serverDirectory))
                throw new DirectoryNotFoundException("Failed to find server root dir in path: " + serverDirectory);

            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(serverDirectory)
                .AddJsonFile("appsettings.Test.json")
                .Build();

            var builder = new WebHostBuilder()
                .UseContentRoot(serverDirectory)
                .UseStartup<Startup>()
               // .UseUrls(BaseUri.AbsoluteUri)
                .UseConfiguration(configurationRoot);

            Configuration = configurationRoot.Providers.FirstOrDefault();

            var server = new TestServer(builder);
            HttpClient = server.CreateClient();
            HttpClient.BaseAddress = server.BaseAddress;;

            var wsc = server.CreateWebSocketClient();
            WebSocket = wsc.ConnectAsync(new Uri(server.BaseAddress, "ws"), CancellationToken.None).Result;
            _buffer = WebSocket.CreateClientBuffer(1024, 1024);

            Task.Run(()=> WebSocket.ReceiveAsync(_buffer, CancellationToken.None));

            EventQueueManager = new EventQueueManager();
            EventQueueManager.Flush();
            EventQueueManager.Setup(Configuration);
        }

        public void Dispose()
        {
            WebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "ok", CancellationToken.None).Wait();
            EventQueueManager?.Dispose();
            HttpClient?.Dispose();
        }
    }
}