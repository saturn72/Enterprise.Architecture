namespace EventBus
{
    public static class EventBusExtensions
    {
        public static void PublishEntityCreatedEvent<TData>(this IEventBus eventBus, TData data)
        {
            BuildAndPublishEvent(eventBus, data, IntegrationEventAction.Created);
        }

        public static void PublishEntityReadEvent<TData>(this IEventBus eventBus, TData data)
        {
            BuildAndPublishEvent(eventBus, data, IntegrationEventAction.Read);
        }

        public static void PublishEntityUpdatedEvent<TData>(this IEventBus eventBus, TData data)
        {
            BuildAndPublishEvent(eventBus, data, IntegrationEventAction.Updated);
        }

        public static void PublishEntityDeletedEvent<TData>(this IEventBus eventBus, TData data)
        {
            BuildAndPublishEvent(eventBus, data, IntegrationEventAction.Delete);
        }

        private static void BuildAndPublishEvent<TData>(this IEventBus eventBus, TData data, IntegrationEventAction integrationEventAction)
        {
            var @event = new IntegrationEvent<TData>(data, integrationEventAction);
            eventBus.Publish(@event);
        }
    }
}