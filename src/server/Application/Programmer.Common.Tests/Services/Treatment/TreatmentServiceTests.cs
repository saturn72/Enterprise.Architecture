using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Programmer.Common.Domain.Treatment;
using Programmer.Common.Services;
using Programmer.Common.Services.Treatment;
using Shouldly;
using Xunit;

namespace Programmer.Common.Tests.Services.Treatment
{
    public class TreatmentServiceTests
    {
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

            var srv = new TreatmentService(tRepo.Object);
            var res = await srv.GetAll();

            res.Result.ShouldBe(ServiceResponseResult.Read);
            res.Data.ShouldBe(dbData);
            res.Message.ShouldBeNull();
        }
    }
}