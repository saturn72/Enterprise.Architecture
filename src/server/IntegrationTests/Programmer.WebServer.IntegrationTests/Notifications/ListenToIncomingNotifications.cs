using Programmer.Test.Framework;
using Xunit;

namespace Programmer.WebServer.IntegrationTests.Notifications
{
    public class ListenToIncomingNotifications:IntegrationTestBase
    {
        [Fact]
        public void PublishIncomingNotificationsViaWebSocket()
        {
            var res = HttpClient.PostAsync("sss", null);
            
        }
    }
}
