using System.Threading;
using System.Threading.Tasks;
using EventBus;

namespace PnpServiceFake.Integration
{
    public class CrudIntegrationEventHandler : IIntegrationEventHandler<CrudIntegrationEvent<CommandRequest>>
    {
        public async Task Handle(CrudIntegrationEvent<CommandRequest> @event)
        {
            await Task.Run(()=> Thread.Sleep(5000));
            //Add to q here
            // Blocked
            // Busy
            // Success
            // Failed
        }
    }
}