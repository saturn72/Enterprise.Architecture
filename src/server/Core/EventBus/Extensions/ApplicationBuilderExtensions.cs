using EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static void SubscribeToIntegrationEvent<TEvent, TEventHandler>(this IApplicationBuilder appBuilder)
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>
        {
            var eventBus = appBuilder.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<TEvent, TEventHandler>();
        }
            
    }
}
