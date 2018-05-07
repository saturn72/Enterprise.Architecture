using System;
using System.Threading;
using System.Threading.Tasks;
using EventBus;
using Programmer.Common.Services.Command;
using Programmer.Test.Framework;
using Xunit;

namespace Programmer.WebServer.IntegrationTests.Notifications
{
    public class ListenToIncomingNotifications:IntegrationTestBase
    {
        [Trait("Category","integration_tests")]
        [Fact]
        public async Task PublishIncomingNotificationsViaWebSocket()
        {
            var cmdReq = new CommandRequest("cmd-req-name", "123123123");
            var cmdRes = new CommandResponse(cmdReq);
            var @event = new IntegrationEvent<CommandResponse>(cmdRes, IntegrationEventAction.Created);

            EventQueueManager.Publish(@event);
            //push to Q
            //Process by Programmer Web
            //Recieve message as client
            Thread.Sleep(2000);
           
            //var res = HttpClient.PostAsync("sss", null);
           // WebSocketClient.
            throw new NotImplementedException();
        }
    }
}
