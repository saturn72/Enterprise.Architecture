using System;
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
        protected static readonly Uri BaseUri = new Uri("http://localhost:5001/");

        protected readonly HttpClient HttpClient;
        protected readonly WebSocket WebSocket;
        protected readonly IConfigurationProvider Configuration;
        protected readonly EventQueueManager EventQueueManager;
        protected readonly byte[] Buffer = new byte[1024];

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
            HttpClient = server.CreateClient();
            HttpClient.BaseAddress = BaseUri;

            WebSocket = server.CreateWebSocketClient().ConnectAsync(new Uri(BaseUri, "ws"), CancellationToken.None).Result;

            Task.Run(()=> WebSocket.ReceiveAsync(Buffer, CancellationToken.None));

            EventQueueManager = new EventQueueManager();
            EventQueueManager.Flush();
            EventQueueManager.Setup(Configuration);
        }

        public void Dispose()
        {
            EventQueueManager?.Dispose();
            HttpClient?.Dispose();
        }
    }
}