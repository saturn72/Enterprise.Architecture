
using System;
using Moq;
using Xunit;

namespace EventBus.Tests
{
    public class EventBusExtensionsTests
    {
        [Fact]
        public void EventBusExtensions_PublishEntityCreatedEvent()
        {
            var entity = "data";
            var eb = new Mock<IEventBus>();
            EventBusExtensions.PublishEntityCreatedEvent(eb.Object, entity);

            eb.Verify(ev => ev.Publish(It.Is<CrudIntegrationEvent<string>>(e => e.CrudAction == CrudAction.Created && e.Entity == entity && e.CreatedOnUtc <= DateTime.UtcNow && e.Id != default(Guid))), Times.Once);
        }

        [Fact]
        public void EventBusExtensions_PublishEntityReadEvent()
        {
            var entity = "data";
            var eb = new Mock<IEventBus>();
            EventBusExtensions.PublishEntityReadEvent(eb.Object, entity);

            eb.Verify(ev => ev.Publish(It.Is<CrudIntegrationEvent<string>>(e => e.CrudAction == CrudAction.Read && e.Entity == entity && e.CreatedOnUtc <= DateTime.UtcNow && e.Id != default(Guid))), Times.Once);
        }

        [Fact]
        public void EventBusExtensions_PublishEntityUpdatedEvent()
        {
            var entity = "data";
            var eb = new Mock<IEventBus>();
            EventBusExtensions.PublishEntityUpdatedEvent(eb.Object, entity);

            eb.Verify(ev => ev.Publish(It.Is<CrudIntegrationEvent<string>>(e => e.CrudAction == CrudAction.Updated && e.Entity == entity && e.CreatedOnUtc <= DateTime.UtcNow && e.Id != default(Guid))), Times.Once);
        }

        [Fact]
        public void EventBusExtensions_PublishEntityDeletedEvent()
        {
            var entity = "data";
            var eb = new Mock<IEventBus>();
            EventBusExtensions.PublishEntityDeletedEvent(eb.Object, entity);

            eb.Verify(ev => ev.Publish(It.Is<CrudIntegrationEvent<string>>(e => e.CrudAction == CrudAction.Delete && e.Entity == entity && e.CreatedOnUtc <= DateTime.UtcNow && e.Id != default(Guid))), Times.Once);
        }
    }
}
