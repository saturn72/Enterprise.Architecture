using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace Calculator.WebServer.Tests.IntegrationTests.Controller
{

    public class RatesControllerIntegTests
    {
        private HttpClient _httpClient;

        public RatesControllerIntegTests()
        {
            var wh = new WebHostBuilder()
                .UseStartup<Startup>();

            _httpClient = new TestServer(wh).CreateClient();
        }

        [Theory]
        [InlineData(0, 2, 3.4, 1.7)]
        [InlineData(1.7, 0, 3.4, 2)]
        [InlineData(1.7, 2, 0, 3.4)]
        public async Task RatesControllerIntegTests_InvalidaRates(decimal rate, decimal dose, decimal vtbi, decimal expected)
        {
            var query = "";

            if (vtbi == 0)
            {
                query += "?rate=" + rate + "&dose=" + dose;
                vtbi = expected;
            }
            else
            {
                if (rate == 0)
                {
                    query = "?vtbi=" + vtbi + "&dose=" + dose;
                    rate = expected;
                }
                else
                {
                    query = "?rate=" + rate + "&vtbi=" + vtbi;
                    dose = expected;
                }
            }

            var res = await _httpClient.GetAsync("api/rates" + query);
            res.EnsureSuccessStatusCode();
            res.StatusCode.ShouldBe(HttpStatusCode.OK);
            var content = await res.Content.ReadAsStringAsync();

            var o = JObject.Parse(content);
            o["vtbi"].Value<decimal>().ShouldBe(vtbi);
            o["rate"].Value<decimal>().ShouldBe(rate);
            o["dose"].Value<decimal>().ShouldBe(dose);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        public async Task RatesControllerIntegTests_ValidaRates(decimal rate, decimal dose, decimal vtbi)
        {
            var query = "?rate=" + rate + "&dose=" + dose + "&vtbi=" + vtbi;
            var res = await _httpClient.GetAsync("api/rates" + query);

            res.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
