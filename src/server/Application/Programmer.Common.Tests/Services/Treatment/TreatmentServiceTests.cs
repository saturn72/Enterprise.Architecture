using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Programmer.Common.Domain.Treatment;
using Programmer.Common.Services;
using Programmer.Common.Services.Command;
using Programmer.Common.Services.Treatment;
using Shouldly;
using Xunit;

namespace Programmer.Common.Tests.Services.Treatment
{
    public class TreatmentServiceTests
    {
        #region GetAll

        public static IEnumerable<object[]> TreatmentService_GetAll_Data =>
            new[]
            {
                new object[] {null},
                new object[]
                {
                    new[]
                    {
                        new TreatmentModel {SessionId = "1"},
                        new TreatmentModel {SessionId = "2"},
                        new TreatmentModel {SessionId = "3"},
                        new TreatmentModel {SessionId = "4"}
                    }
                }
            };

        [Theory]
        [MemberData(nameof(TreatmentService_GetAll_Data))]
        public async Task TreatmentService_GetAll(IEnumerable<TreatmentModel> dbData)
        {
            var tRepo = new Mock<ITreatmentRepository>();
            tRepo.Setup(t => t.GetAll())
                .Returns(dbData);

            var srv = new TreatmentService(tRepo.Object, null);
            var res = await srv.GetAll();

            res.Result.ShouldBe(ServiceResponseResult.Read);
            res.Data.ShouldBe(dbData);
            res.Message.ShouldBeNull();
        }
        #endregion
        #region GetById

        public static IEnumerable<object[]> TreatmentService_GetById_Data =>
            new[]
            {
                new object[] {null, ServiceResponseResult.NotFound},
                new object[]
                {
                        new TreatmentModel {SessionId = "1"},ServiceResponseResult.Read
                }
            };

        [Theory]
        [MemberData(nameof(TreatmentService_GetById_Data))]
        public async Task TreatmentService_GetById(TreatmentModel dbModel, ServiceResponseResult expServiceResult)
        {
            var tRepo = new Mock<ITreatmentRepository>();
            tRepo.Setup(t => t.GetById(It.IsAny<long>()))
                .Returns(dbModel);

            var srv = new TreatmentService(tRepo.Object, null);
            var res = await srv.GetById(12);

            res.Result.ShouldBe(expServiceResult);
            res.Data.ShouldBe(dbModel);
            res.Message.ShouldBeNull();
        }

        #endregion
        #region Create Treament

        [Theory]
        [InlineData(ServiceResponseResult.Accepted)]
        [InlineData(ServiceResponseResult.NotAcceptable)]
        public async Task TreatmentService_CrateTreament(ServiceResponseResult serviceResponseResult)
        {
            var expectedMsg = "this is message";
            var tModel = new TreatmentModel
            {
                SessionId = "session-id",
                Dose = 1,
                Vtbi = 3,
                Rate = 13
            };
            var expectedServiceResponse = new ServiceResponse<TreatmentModel>
            {
                Data = tModel,
                Result = serviceResponseResult,
                Message = expectedMsg
            };
            Func<CommandRequest, ServiceResponse<CommandResponse>> cmdServiceResponse = cmdReq =>
                new ServiceResponse<CommandResponse>
                {
                    Data = new CommandResponse(cmdReq),
                    Message = expectedMsg,
                    Result = serviceResponseResult
                };

            var tRepo = new Mock<ITreatmentRepository>();
            var cmdManager = new Mock<ICommandManager>();
            cmdManager.Setup(t => t.SendCommand(It.IsAny<CommandRequest>()))
                .Returns<CommandRequest>(cr => Task.FromResult(cmdServiceResponse(cr)));

            var srv = new TreatmentService(tRepo.Object, cmdManager.Object);
            var res = await srv.CreateTreament(tModel);

            res.Data.ShouldBe(expectedServiceResponse.Data);
            res.Result.ShouldBe(expectedServiceResponse.Result);
            res.Message.ShouldBe(expectedServiceResponse.Message);

            if (serviceResponseResult == ServiceResponseResult.Accepted)
                tRepo.Verify(t => t.CreateTreatment(It.Is<TreatmentModel>(tm => tm == tModel)), Times.Once);
        }

        #endregion
    }
}