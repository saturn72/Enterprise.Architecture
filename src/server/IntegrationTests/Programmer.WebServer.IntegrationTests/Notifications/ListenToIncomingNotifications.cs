using System;
using Programmer.Test.Framework;
using Xunit;

namespace Programmer.WebServer.IntegrationTests.Notifications
{
    public class ListenToIncomingNotifications:IntegrationTestBase
    {
        [Trait("Category","integration_tests")]
        [Fact]
        public void PublishIncomingNotificationsViaWebSocket()
        {
            EventQueueManager.Publish("someData");
            //push to Q
            //Process by Programmer Web
            //Recieve message as client

            var res = HttpClient.PostAsync("sss", null);
            

            throw new NotImplementedException();
        }
    }
}
