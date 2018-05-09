using Calculator.WebServer.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace Calculator.WebServer.Tests.Controllers
{
    public class RatesControllerTests
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        public void RateControllerTests_MissingData(decimal rate, decimal dose, decimal vtbi)
        {
            var ctrl = new RatesController();
            var res = ctrl.GetRates(rate, vtbi, dose);

            res.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Theory]
        [InlineData(0, 2, 3.4, 1.7)]
        [InlineData(1.7, 0, 3.4, 2)]
        [InlineData(1.7, 2, 0, 3.4)]
        public void RateControllerTests_GetRates(decimal rate, decimal dose, decimal vtbi, decimal expected)
        {
            var ctrl = new RatesController();
            var res = ctrl.GetRates(rate, dose, vtbi);

            if (vtbi == 0)
            { vtbi = expected; }
            else
            {
                if (rate == 0)
                    rate = expected;
                else
                {
                        dose = expected;
                }
            }

            var ok = res.ShouldBeOfType<OkObjectResult>();

            var o = (JObject)JToken.FromObject(ok.Value);
            o["vtbi"].Value<decimal>().ShouldBe(vtbi);
            o["rate"].Value<decimal>().ShouldBe(rate);
            o["dose"].Value<decimal>().ShouldBe(dose);
        }
    }
}
