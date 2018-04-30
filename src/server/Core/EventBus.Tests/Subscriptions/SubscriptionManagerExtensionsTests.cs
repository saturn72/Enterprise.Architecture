using System;
using EventBus.Subscriptions;
using Moq;
using Shouldly;
using Xunit;

namespace EventBus.Tests.Subscriptions
{
    public class SubscriptionManagerExtensionsTests
    {
        [Fact]
        public void SubscriptionManagerExtensions_GetEventKey()
        {
            var sm = new Mock<ISubscriptionsManager>();
            var key = SubscriptionsManagerExtensions.GetEventKey<string>(sm.Object);
            key.ShouldBe("string");
            sm.Verify(s => s.GetEventKey(It.Is<Type>(t => t == typeof(string))), Times.Once);
        }

        [Fact]
        public void SessionManagerExtensionsTests_HasSubscriptionsForEvent()
        {
            var typeName = "tName";
            var sm = new Mock<ISubscriptionsManager>();
            sm.Setup(s => s.GetEventKey(It.IsAny<Type>())).Returns(typeName);

            SubscriptionsManagerExtensions.HasSubscriptionsForEvent<TestIntegrationEvent>(sm.Object);
            sm.Verify(s => s.HasSubscriptionsForEvent(It.Is<string>(s1 => s1 == typeName)), Times.Once);
        }


        [Fact]
        public void SessionManagerExtensionsTests_GetHandlersForEvent()
        {
            var typeName = "tName";
            var sm = new Mock<ISubscriptionsManager>();
            sm.Setup(s => s.GetEventKey(It.IsAny<Type>())).Returns(typeName);

            SubscriptionsManagerExtensions.GetHandlersForEvent<TestIntegrationEvent>(sm.Object);
            sm.Verify(s=>s.GetHandlersForEvent(It.Is<string>(s1=> s1 == typeName)), Times.Once);
        }

        internal class TestIntegrationEvent : IntegrationEvent
        {

        }

}
}
