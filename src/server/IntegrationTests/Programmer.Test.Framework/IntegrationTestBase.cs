using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Programmer.WebServer;

namespace Programmer.Test.Framework
{
    public abstract class IntegrationTestBase
    {
        protected static readonly Uri BaseUri = new Uri("http://localhost:5001/");
        protected readonly HttpClient Client;

        protected IntegrationTestBase()
        {
            var path = PlatformServices.Default.Application.ApplicationBasePath;
            var serverDirectory = Path.GetFullPath(Path.Combine(path, @"../../../../../Programmer.WebServer"));
            if(!Directory.Exists(serverDirectory))
                throw new DirectoryNotFoundException("Failed to find server root dir in path: " + serverDirectory);

            var builder = new WebHostBuilder()
                .UseContentRoot(serverDirectory)
                .UseStartup<Startup>()
                .UseUrls(BaseUri.AbsoluteUri)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(serverDirectory)
                    .AddJsonFile("appsettings.Test.json")
                    .Build()
                );

            var server = new TestServer(builder);
            var t = server.BaseAddress;
            Client = server.CreateClient();
            Client.BaseAddress = BaseUri;
        }
    }
}
