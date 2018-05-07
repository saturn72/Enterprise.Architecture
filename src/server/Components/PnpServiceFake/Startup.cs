using System;
using System.IO;
using System.Reflection;
using EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PnpServiceFake.Integration;
using Swashbuckle.AspNetCore.Swagger;

namespace PnpServiceFake
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Programmer API Tester", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
            });

            services.RegisterRabbitMqPublisher(Configuration);

            //subscribe to integration event
            services.AddTransient<IIntegrationEventHandler<IntegrationEvent<CommandRequest>>>(sp =>
            {
                var eb = sp.GetService<IEventBus>();
                return new IntegrationEventHandler(eb);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Swashbucke
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Programmer API Tester");
            });

            var eventBus = app.ApplicationServices.GetService<IEventBus>();
            eventBus.Subscribe<IntegrationEvent<CommandRequest>, IntegrationEventHandler>();
            app.UseMvc();
        }
    }
}
