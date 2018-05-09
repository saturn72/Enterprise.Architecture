using System.Linq;
using System.Threading;
using EventBus;
using Programmer.Common.Services.Command;
using Programmer.Test.Framework;
using Shouldly;
using Xunit;

namespace Programmer.WebServer.IntegrationTests.Notifications
{
    public class ListenToIncomingNotifications : IntegrationTestBase
    {
        [Trait("Category", "integration_tests")]
        [Fact]
        public void PublishIncomingNotificationsViaWebSocket()
        {
            var cmdReq = new CommandRequest("cmd-req-name", "123123123");
            var cmdRes = new CommandResponse(cmdReq);
            var @event = new IntegrationEvent<CommandResponse>(cmdRes, IntegrationEventAction.Created);

            EventQueueManager.Publish(@event);
            Thread.Sleep(250);

            Buffer.Any(x => x != 0).ShouldBeTrue();
        }
    }
}
