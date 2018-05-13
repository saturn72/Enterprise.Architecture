using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Programmer.Test.Framework;
using Programmer.Test.Framework.Models;
using Programmer.Test.Framework.Q;
using Shouldly;
using Xunit;

namespace Programmer.WebServer.IntegrationTests.Treatment
{
    public class TreatmentApi : IntegrationTestBase
    {
        private const string RestResource = "/api/treatment/";

        [Trait("Category", "integration_tests")]
        [Trait("Category", "create_treament")]
        [Fact]
        public async Task CreateTreatment_AddToQ()
        {
            var treatment = new
            {
                vtbi = 123,
                rate = 456,
                dose = 5364
            };

            var req = RestUtil.BuildRequest(HttpMethod.Post, RestResource, treatment);
            req.Headers.Add("X-Session-Token", "some-session-id");

            var res = await HttpClient.SendAsync(req);
            res.EnsureSuccessStatusCode();
            res.IsSuccessStatusCode.ShouldBeTrue();
            res.StatusCode.ShouldBe(HttpStatusCode.Accepted);
            EventQueueManager.IncomingEvents.Any().ShouldBeTrue();
        }

        [Trait("Category", "integration_tests")]
        [Trait("Category", "create_treament")]
        [Fact]
        public async Task CreateTreatment_MissingXSessionToken()
        {
            var treatment = new
            {
                vtbi = 123,
                rate = 456,
                dose = 5364
            };

            var req = RestUtil.BuildRequest(HttpMethod.Post, RestResource, treatment);
            var res = await HttpClient.SendAsync(req);
            res.IsSuccessStatusCode.ShouldBeFalse();
            res.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Trait("Category", "integration_tests")]
        [Trait("Category", "read_treatment")]
        [Fact]
        public async Task CreateTreatment_ReadAll()
        {
            var expTreatments = new List<TreatmentTestModel>();
            var random = new Random(1024);
            for (var i = 0; i < 5; i++)
            {
                var treatment = new TreatmentTestModel
                {
                    Vtbi = random.Next(500),
                    Rate = random.Next(500),
                    Dose = random.Next(500),
                    Id = default(long)
                };
                expTreatments.Add(treatment);

                var req = RestUtil.BuildRequest(HttpMethod.Post, RestResource, treatment);
                req.Headers.Add("X-Session-Token", "some-session-id");
                var res = await HttpClient.SendAsync(req);
                var addResponseContent = await RestUtil.ExtractJObject(res);
                treatment.Id = addResponseContent["data"]["id"].Value<long>();
            }

            var getAllReq = RestUtil.BuildRequest(HttpMethod.Get, RestResource, null);

            var getAllRes = await HttpClient.SendAsync(getAllReq);
            getAllRes.EnsureSuccessStatusCode();

            var content = await RestUtil.ExtractJObject(getAllRes);
            var models = content.Value<JArray>("data").ToObject<TreatmentTestModel[]>();

            models.Length.ShouldBeGreaterThanOrEqualTo(expTreatments.Count);

            foreach (var et in expTreatments)
            {
                models.Count(r => r.Id == et.Id).ShouldBe(1);
                models.Any(r =>
                    r.SessionId == et.SessionId
                    && r.Id == et.Id
                    && r.Rate == et.Rate
                    && r.Vtbi == et.Vtbi
                    && r.Dose == et.Dose).ShouldBeTrue();
            }
        }

        [Trait("Category", "integration_tests")]
        [Trait("Category", "read_treatment")]
        [Fact]
        public async Task CreateTreatment_ReadById()
        {
            var random = new Random(1024);
            var expTreatment = new TreatmentTestModel
            {
                Vtbi = random.Next(500),
                Rate = random.Next(500),
                Dose = random.Next(500),
                Id = default(long)
            };

            var req = RestUtil.BuildRequest(HttpMethod.Post, RestResource, expTreatment);
            req.Headers.Add("X-Session-Token", "some-session-id");
            var res = await HttpClient.SendAsync(req);
            var addResponseContent = await RestUtil.ExtractJObject(res);
            expTreatment.Id = addResponseContent["data"]["id"].Value<long>();

            var getByIdReq = RestUtil.BuildRequest(HttpMethod.Get, RestResource + expTreatment.Id, null);

            var getByIdRes = await HttpClient.SendAsync(getByIdReq);
            getByIdRes.EnsureSuccessStatusCode();

            var content = await RestUtil.ExtractJObject(getByIdRes);
            var data = content["data"];

            data.Value<string>("sessionId").ShouldBe(expTreatment.SessionId);
            data.Value<long>("id").ShouldBe(expTreatment.Id);
            data.Value<decimal>("rate").ShouldBe(expTreatment.Rate);
            data.Value<decimal>("vtbi").ShouldBe(expTreatment.Vtbi);
            data.Value<decimal>("dose").ShouldBe(expTreatment.Dose);
        }

        [Trait("Category", "integration_tests")]
        [Trait("Category", "read_treatment")]
        [Fact]
        public async Task CreateTreatment_ReadById_NotFound()
        {
            var notFoundRequest = RestUtil.BuildRequest(HttpMethod.Get, RestResource + long.MaxValue, null);
            var notFoundResponse = await HttpClient.SendAsync(notFoundRequest);
            notFoundResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}