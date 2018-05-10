using System;

namespace EventBus
{
    public class IntegrationEvent
    {
        #region ctor
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedOnUtc = DateTime.UtcNow;
        }

        #endregion

        #region Properties

        public Guid Id { get; }
        public DateTime CreatedOnUtc { get; }

        #endregion
    }

    public class IntegrationEvent<TEntity> : IntegrationEvent
    {
        public IntegrationEvent(TEntity entity, IntegrationEventAction integrationEventAction)
        {
            Entity = entity;
            IntegrationEventAction = integrationEventAction;
        }
        public TEntity Entity { get; }

        public IntegrationEventAction IntegrationEventAction { get; }
    }
}
