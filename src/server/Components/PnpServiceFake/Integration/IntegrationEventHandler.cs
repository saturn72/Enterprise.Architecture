using System.Threading;
using System.Threading.Tasks;
using EventBus;

namespace PnpServiceFake.Integration
{
    public class IntegrationEventHandler : IIntegrationEventHandler<IntegrationEvent<CommandRequest>>
    {
        private readonly IEventBus _eventBus;

        public IntegrationEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task Handle(IntegrationEvent<CommandRequest> @event)
        {
            //mimic processing time
            await Task.Run(() => Thread.Sleep(5000));
            var cmdReponse = new CommandResponse(@event.Entity);
            var processedEvent = new IntegrationEvent<CommandResponse>(cmdReponse, IntegrationEventAction.Processed);
            _eventBus.Publish(processedEvent);
        }
    }
}