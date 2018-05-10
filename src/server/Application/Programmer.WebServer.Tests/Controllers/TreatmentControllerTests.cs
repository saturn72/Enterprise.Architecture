using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Programmer.Common.Domain.Treatment;
using Programmer.Common.Services;
using Programmer.Common.Services.Treatment;
using Programmer.WebServer.Controllers;
using Shouldly;
using Xunit;

namespace Programmer.WebServer.Tests.Controllers
{
    public class TreatmentControllerTests
    {
        [Fact]
        public async Task TreatmentController_GetAll()
        {
            var data = new[]
            {
                new TreatmentModel{SessionId = "1"},
                new TreatmentModel{SessionId = "2"},
                new TreatmentModel{SessionId = "3"},
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
