namespace EventBus
{
    public class CrudIntegrationEvent<TEntity> : IntegrationEvent
    {
        public TEntity Entity { get; internal set; }

        public CrudAction CrudAction { get; internal set; }
    }
}