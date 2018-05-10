using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Programmer.Common.Domain.Treatment;
using Programmer.Common.Services;
using Programmer.Common.Services.Command;
using Programmer.WebServer.Controllers;
using Shouldly;
using Xunit;

namespace Programmer.WebServer.Tests.Controllers
{
    public class CommandControllerTests
    {
        [Fact]
        public async Task CommandController_SetTreatment_BadRequestOnNullModel()
        {
            var controller = new CommandController(null);
            var res = await controller.SetTreatment(null);
            res.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CommandController_SetTreatment_BadRequestOnMissingSessionIdHeader()
        {
            var controller = new CommandController(null);
            var res = await controller.SetTreatment(new TreatmentModel());
            res.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task CommandController_SetTreatment_BadRequestOnEmptySessionIdHeader(string sessionId)
        {
            var controller = new CommandController(null);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers[CommandController.PumpSessionHeaderName] = sessionId;
            var res = await controller.SetTreatment(new TreatmentModel());

            res.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CommandController_SetTreatment_NotAcceptable()
        {
            var tm = new TreatmentModel();
            var srvRes = new ServiceResponse<CommandResponse>
            {
                Message = "some-err",
                Result = ServiceResponseResult.NotAcceptable
            };

            var cmdSrv = new Mock<ICommandService>();
            cmdSrv.Setup(c => c.SendCommand(It.IsAny<CommandRequest>())).ReturnsAsync(srvRes);

            var controller = new CommandController(cmdSrv.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers[CommandController.PumpSessionHeaderName] = "some-session-id";
            var res = await controller.SetTreatment(tm);

            var or = res.ShouldBeOfType<ObjectResult>();
            or.StatusCode.ShouldBe((int)HttpStatusCode.NotAcceptable);
        }

    }
}