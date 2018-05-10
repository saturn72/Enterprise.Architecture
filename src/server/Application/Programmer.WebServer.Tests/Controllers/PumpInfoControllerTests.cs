using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Programmer.Common.Domain.Pump;
using Programmer.Common.Services;
using Programmer.Common.Services.Pump;
using Programmer.WebServer.Controllers;
using Shouldly;
using Xunit;

namespace Programmer.WebServer.Tests.Controllers
{
    public class PumpInfoControllerTests
    {
        [Theory]
        [InlineData("   ")]
        [InlineData("")]
        [InlineData(null)]
        public async Task PumpInfoController_GetById_BadRequestOnIllegalId(string id)
        {
            var ctrl = new PumpInfoController(null);
            var res = await ctrl.GetById(id);
            res.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PumpInfoController_GetById_NotFound()
        {
            var piSrv = new Mock<IPumpInfoService>();
            var srvResponse = new ServiceResponse<PumpInfoModel>
            {
                Result = ServiceResponseResult.NotFound
            };

            piSrv.Setup(s => s.GetPumpInfoById(It.IsAny<string>())).ReturnsAsync(srvResponse);
            var ctrl = new PumpInfoController(piSrv.Object);
            var res = await ctrl.GetById("some-id");
            res.ShouldBeOfType<NotFoundObjectResult>();
        }
        [Fact]
        public async Task PumpInfoController_GetById_Ok()
        {
            var srvData = new PumpInfoModel
            {
                Id = "123"
            };

            var piSrv = new Mock<IPumpInfoService>();
            var srvResponse = new ServiceResponse<PumpInfoModel>
            {
                Data = srvData,
                Result = ServiceResponseResult.Success
            };

            piSrv.Setup(s => s.GetPumpInfoById(It.IsAny<string>())).ReturnsAsync(srvResponse);
            var ctrl = new PumpInfoController(piSrv.Object);
            var res = await ctrl.GetById("some-id");
            res.ShouldBeOfType<OkObjectResult>();
        }
    }
}