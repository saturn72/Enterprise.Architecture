namespace EventBus
{
    public static class EventBusExtensions
    {
        public static void PublishEntityCreatedEvent<TData>(this IEventBus eventBus, TData data)
        {
            BuildAndPublishEvent(eventBus, data, CrudAction.Created);
        }

        public static void PublishEntityReadEvent<TData>(this IEventBus eventBus, TData data)
        {
            BuildAndPublishEvent(eventBus, data, CrudAction.Read);
        }

        public static void PublishEntityUpdatedEvent<TData>(this IEventBus eventBus, TData data)
        {
            BuildAndPublishEvent(eventBus, data, CrudAction.Updated);
        }

        public static void PublishEntityDeletedEvent<TData>(this IEventBus eventBus, TData data)
        {
            BuildAndPublishEvent(eventBus, data, CrudAction.Delete);
        }

        private static void BuildAndPublishEvent<TData>(this IEventBus eventBus, TData data, CrudAction crudAction)
        {
            var @event = new CrudIntegrationEvent<TData>
            {
                Entity = data,
                CrudAction = crudAction
            };
            eventBus.Publish(@event);
        }
    }
}