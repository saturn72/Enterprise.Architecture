using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Programmer.Common.Domain.Treatment;
using Programmer.Common.Services;
using Programmer.Common.Services.Command;
using Programmer.Common.Services.Treatment;
using Programmer.WebServer.Controllers;
using Shouldly;
using Xunit;

namespace Programmer.WebServer.Tests.Controllers
{
    public class TreatmentControllerTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task TreatmentController_CreateTreatment_BadRequestOnEmptySessionIdHeader(string sessionId)
        {
            var controller = new TreatmentController(null);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers[TreatmentController.PumpSessionHeaderName] =
                sessionId;
            var res = await controller.CreateTreatment(new TreatmentModel());

            res.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task TreatmentController_CreateTreatment_BadRequestOnMissingSessionIdHeader()
        {
            var controller = new TreatmentController(null);
            var res = await controller.CreateTreatment(new TreatmentModel());
            res.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task TreatmentController_CreateTreatment_BadRequestOnNullModel()
        {
            var controller = new TreatmentController(null);
            var res = await controller.CreateTreatment(null);
            res.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task TreatmentController_CreateTreatment_NotAcceptable()
        {
            var tm = new TreatmentModel();
            var srvRes = new ServiceResponse<TreatmentModel>
            {
                Message = "some-err",
                Result = ServiceResponseResult.NotAcceptable
            };

            var tSrv = new Mock<ITreatmentService>();
            tSrv.Setup(c => c.CreateTreament(It.IsAny<TreatmentModel>())).ReturnsAsync(srvRes);

            var controller = new TreatmentController(tSrv.Object)
            {
                ControllerContext = new ControllerContext {HttpContext = new DefaultHttpContext()}
            };
            controller.ControllerContext.HttpContext.Request.Headers[TreatmentController.PumpSessionHeaderName] =
                "some-session-id";
                var res = await controller.CreateTreatment(tm);

            var or = res.ShouldBeOfType<ObjectResult>();
            or.StatusCode.ShouldBe((int) HttpStatusCode.NotAcceptable);
        }

        [Fact]
        public async Task TreatmentController_GetAll()
        {
            var data = new[]
            {
                new TreatmentModel {SessionId = "1"},
                new TreatmentModel {SessionId = "2"},
                new TreatmentModel {SessionId = "3"}
            };
            var srvRes = new ServiceResponse<IEnumerable<TreatmentModel>>
            {
                Result = ServiceResponseResult.Read,
                Data = data,
                Message = "some-message"
            };

            var tSrv = new Mock<ITreatmentService>();
            tSrv.Setup(ts => ts.GetAll())
                .ReturnsAsync(srvRes);

            var ctrl = new TreatmentController(tSrv.Object);
            var res = await ctrl.ReadAll();

            var response = res.ShouldBeOfType<OkObjectResult>();
        }
    }
}