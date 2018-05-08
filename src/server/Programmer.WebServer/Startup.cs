using System;
using System.IO;
using System.Reflection;
using CacheManager;
using EventBus;
using MemoryCacheManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Programmer.Common.Fakes.Command;
using Programmer.Common.Fakes.Session;
using Programmer.Common.Services.Command;
using Programmer.Common.Services.Pump;
using Programmer.Common.Services.Session;
using Programmer.WebServer.Integration;
using Programmer.WebServer.Swagger;
using Swashbuckle.AspNetCore.Swagger;

namespace Programmer.WebServer
{
    /// <summary>
    ///     Kastrel Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Startup ctor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Programmer API Tester", Version = "v1"});
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
                c.OperationFilter<AddRequiredHeaderParameters>();
            });

            RegisterDependencies(services);
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            services.AddSingleton<ICacheManager, InMemoryCacheManager>();
            services.RegisterRabbitMqPublisher(Configuration);
            //TODO:  generic way to register EventHandlers
            services
                .AddTransient<IIntegrationEventHandler<IntegrationEvent<CommandResponse>>, IntegrationEventHandler>();

            RegisterInternalServices(services);
            var restBaseUri = new Uri("http://localhost:3004");
            services.AddScoped<IPumpInfoRepository>(s => new HttpPumpInfoRepository(restBaseUri));
        }

        private static void RegisterInternalServices(IServiceCollection services)
        {
            services.AddScoped<IPumpInfoService, PumpInfoService>();
            services.AddScoped<ICommandService, CommandService>();
            services.AddScoped<ISessionManager, DummySessionManager>();
            services.AddSingleton<ICommandVerifier, DummyCommandVerifier>();
        }

        /// <summary>
        ///     Configures Pipeline
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            //Swashbucke
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Programmer API Tester"); });


            SetupWebSocket(app);

            var eventBus = app.ApplicationServices.GetService<IEventBus>();
            eventBus.Subscribe<IntegrationEvent<CommandResponse>, IntegrationEventHandler>();
            app.UseMvc();
        }

        private void SetupWebSocket(IApplicationBuilder app)
        {
            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await WebSocketOutlet.Send(webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                else
                    await next();
            });
        }
    }
}