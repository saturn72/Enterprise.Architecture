using System.Collections.Generic;

namespace EventBus.Subscriptions
{
    public static class SubscriptionsManagerExtensions
    {
        public static string GetEventKey<T>(this ISubscriptionsManager subscriptionsManager)
        {
            return subscriptionsManager.GetEventKey(typeof(T));
        }

        public static IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>(this ISubscriptionsManager subscriptionsManager) where T : IntegrationEvent
        {
            var eventName = subscriptionsManager.GetEventKey<T>();
            return subscriptionsManager.GetHandlersForEvent(eventName);
        }

        public static bool HasSubscriptionsForEvent<T>(this ISubscriptionsManager subscriptionsManager) where T : IntegrationEvent
        {
            var eventName = subscriptionsManager.GetEventKey<T>();
            return subscriptionsManager.HasSubscriptionsForEvent(eventName);
        }

    }
}