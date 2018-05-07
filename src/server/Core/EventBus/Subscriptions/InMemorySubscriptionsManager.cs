using System;
using System.Collections.Generic;
using System.Linq;

namespace EventBus.Subscriptions
{
    public class InMemorySubscriptionsManager : ISubscriptionsManager, IInMemorySubscriptionsManager
    {
        private readonly IDictionary<string, ICollection<SubscriptionInfo>> _handlers;
        private readonly IDictionary<string, Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public InMemorySubscriptionsManager()
        {
            _handlers = new Dictionary<string, ICollection<SubscriptionInfo>>();
            _eventTypes = new Dictionary<string, Type>();
        }

        public bool IsEmpty => !_handlers.Keys.Any();
        public void Clear() => _handlers.Clear();

        public void AddDynamicSubscription<TDynamicIntegrationEventHandler>(string eventName)
            where TDynamicIntegrationEventHandler : IDynamicIntegrationEventHandler
        {
            DoAddSubscription(typeof(IDynamicIntegrationEventHandler), typeof(TDynamicIntegrationEventHandler), eventName, isDynamic: true);
        }

        public void AddSubscription<TIntegrationEvent, TIntegrationEventHandler>()
            where TIntegrationEvent : IntegrationEvent
            where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent>
        {
            var eventName = this.GetEventKey<TIntegrationEvent>();
            DoAddSubscription(typeof(IIntegrationEventHandler<TIntegrationEvent>), typeof(TIntegrationEventHandler), eventName, isDynamic: false);
            var iet = typeof(TIntegrationEvent);
            _eventTypes[iet.ToCSharpName()] = iet;
        }

        private void DoAddSubscription(Type handlerServiceType, Type handlerImplType, string key, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(key))
            {
                _handlers.Add(key, new List<SubscriptionInfo>());
            }

            if (_handlers[key].Any(s => s.HandlerImplType == handlerImplType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerImplType.Name} already registered for '{key}'", nameof(handlerImplType));
            }

            if (isDynamic)
            {
                _handlers[key].Add(SubscriptionInfo.AsDynamic(handlerServiceType, handlerImplType));
            }
            else
            {
                _handlers[key].Add(SubscriptionInfo.AsTyped(handlerServiceType, handlerImplType));
            }
        }


        public void RemoveDynamicSubscription<TDynamicIntegrationEventHandler>(string eventName)
            where TDynamicIntegrationEventHandler : IDynamicIntegrationEventHandler
        {
            var handlerToRemove = FindDynamicSubscriptionToRemove<TDynamicIntegrationEventHandler>(eventName);
            DoRemoveHandler(eventName, handlerToRemove);
        }


        public void RemoveSubscription<TIntegrationEvent, TIntegrationEventHandler>()
            where TIntegrationEvent : IntegrationEvent
            where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent>
        {
            var handlerToRemove = FindSubscriptionToRemove<TIntegrationEvent, TIntegrationEventHandler>();
            var eventName = this.GetEventKey<TIntegrationEvent>();
            DoRemoveHandler(eventName, handlerToRemove);
        }


        private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove != null)
            {
                _handlers[eventName].Remove(subsToRemove);
                if (!_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);
                    if (_eventTypes.ContainsKey(eventName))
                        _eventTypes.Remove(eventName);
                    RaiseOnEventRemoved(eventName);
                }
            }
        }


        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName)
        {
            return _handlers.TryGetValue(eventName, out ICollection<SubscriptionInfo> value) ? value : null;
        }

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            if (handler != null)
            {
                OnEventRemoved(this, eventName);
            }
        }


        private SubscriptionInfo FindDynamicSubscriptionToRemove<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }


        private SubscriptionInfo FindSubscriptionToRemove<T, TH>()
             where T : IntegrationEvent
             where TH : IIntegrationEventHandler<T>
        {
            var eventName = this.GetEventKey<T>();
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                return null;
            }

            return _handlers[eventName]
                .SingleOrDefault(s => s.HandlerServiceType == handlerType || s.HandlerImplType == handlerType);

        }


        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        public Type GetEventTypeByName(string eventName)
        {
            _eventTypes.TryGetValue(eventName, out var eventType);
            return eventType;
        }

        public string GetEventKey(Type eventType)
        {
            return eventType.ToCSharpName();
        }
    }
}
