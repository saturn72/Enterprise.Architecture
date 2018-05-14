using System;

namespace EventBus.Subscriptions
{
    public class SubscriptionInfo
    {
        public bool IsDynamic { get; }
        public Type HandlerServiceType { get; }
        public Type HandlerImplType { get; }

        private SubscriptionInfo(bool isDynamic, Type handlerServiceType, Type handlerImplType)
        {
            IsDynamic = isDynamic;
            HandlerServiceType = handlerServiceType;
            HandlerImplType = handlerImplType;
        }

        public static SubscriptionInfo AsDynamic(Type handlerServiceType, Type handlerImplType)
        {
            return new SubscriptionInfo(true, handlerServiceType, handlerImplType);
        }
        public static SubscriptionInfo AsTyped(Type handlerServiceType, Type handlerImplType)
        {
            return new SubscriptionInfo(false, handlerServiceType, handlerImplType);
        }
    }
}