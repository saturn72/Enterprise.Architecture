using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Programmer.Test.Framework;
using Shouldly;
using Xunit;

namespace Programmer.WebServer.IntegrationTests.Commands
{
    public class TreatmentCommand : IntegrationTestBase
    {
        [Fact]
        public async Task SetTreatment_MissingXSessionToken()
        {
            var treatment = new
            {
                vtbi = 123,
                rate = 456,
                dose = 5364
            };

            var req = RestUtil.BuildRequest(HttpMethod.Post, "/api/command/treatment", treatment);
            var res = await HttpClient.SendAsync(req);
            res.IsSuccessStatusCode.ShouldBeFalse();
            res.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task SetTreatment_AddToQ()
        {
            var treatment = new
            {
                vtbi = 123,
                rate = 456,
                dose = 5364
            };

            var req = RestUtil.BuildRequest(HttpMethod.Post, "/api/command/treatment", treatment);
            req.Headers.Add("X-Session-Token", "some-session-id");

            var res = await HttpClient.SendAsync(req);
            res.EnsureSuccessStatusCode();
            res.IsSuccessStatusCode.ShouldBeTrue();
            res.StatusCode.ShouldBe(HttpStatusCode.Accepted);
            QEventListener.QEvents.Any().ShouldBeTrue();
        }
    }
}
