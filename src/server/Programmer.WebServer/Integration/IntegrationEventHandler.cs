using System.Threading.Tasks;
using EventBus;
using Programmer.Common.Services.Command;

namespace Programmer.WebServer.Integration
{
    public class IntegrationEventHandler : IIntegrationEventHandler<IntegrationEvent<CommandResponse>>
    {
        public async Task Handle(IntegrationEvent<CommandResponse> @event)
        {
          await Task.Run(()=> WebSocketOutlet.AddToBuffer("just recieved event: " + @event.ToString()));
        }
    }
}