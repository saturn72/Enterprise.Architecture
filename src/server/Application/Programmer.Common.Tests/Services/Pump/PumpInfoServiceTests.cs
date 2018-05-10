using System.Collections.Generic;
using System.Threading.Tasks;
using CacheManager;
using Moq;
using Programmer.Common.Domain.Pump;
using Programmer.Common.Services;
using Programmer.Common.Services.Pump;
using Shouldly;
using Xunit;

namespace Programmer.Common.Tests.Services.Pump
{
    public class PumpInfoServiceTests
    {
        [Fact]
        public void PumpInfoService_RestResourceUri()
        {
            new TestPumpInfoService(null, null).GetRestResource().ShouldBe("api/pumpinfo");
        }

        #region Get by Id
        private const string PumpInfoId = "some-legal-pump-info-id";

        [Fact]
        public async Task PumpInfoService_GetPumpInfoById_WrongFormatId()
        {
            var srv = new PumpInfoService(null, null);
            var res = await srv.GetPumpInfoById("id with spaces");
            res.Result.ShouldBe(ServiceResponseResult.BadOrMissingData);
        }

        [Theory]
        [MemberData(nameof(PumpInfoService_GetPumpInfoById_NotCached_Data))]
        public async Task PumpInfoService_GetPumpInfoById_NotCached(PumpInfoModel repoModel, ServiceResponseResult expResult)
        {
            var cm = new Mock<ICacheManager>();
            cm.Setup(c => c.Get<PumpInfoModel>(It.IsAny<string>())).Returns(null as PumpInfoModel);

            var pir = new Mock<IPumpInfoRepository>();
            pir.Setup(p => p.GetById(It.IsAny<string>())).ReturnsAsync(repoModel);

            var srv = new PumpInfoService(cm.Object, pir.Object);
            var res = await srv.GetPumpInfoById(PumpInfoId);
            res.Result.ShouldBe(expResult);
        }
        public static IEnumerable<object[]> PumpInfoService_GetPumpInfoById_NotCached_Data =>
            new[]{
                new object[] { null as PumpInfoModel, ServiceResponseResult.NotFound },
                new object[] { new PumpInfoModel{Id = "123"}, ServiceResponseResult.Success },
            };


        [Fact]
        public async Task PumpInfoService_GetPumpInfoById_Cached()
        {
            var expModel = new PumpInfoModel { Id = "123" };

            var cm = new Mock<ICacheManager>();
            cm.Setup(c => c.Get<PumpInfoModel>(It.IsAny<string>())).Returns(expModel);

            var srv = new PumpInfoService(cm.Object, null);
            var res = await srv.GetPumpInfoById(PumpInfoId);
            res.Data.ShouldBe(expModel);
            res.Result.ShouldBe(ServiceResponseResult.Success);
        }


        #endregion
        internal class TestPumpInfoService : PumpInfoService
        {
            public TestPumpInfoService(ICacheManager cacheManager, IPumpInfoRepository pumpInfoRepository) : base(cacheManager, pumpInfoRepository)
            {
            }

            internal string GetRestResource()
            {
                return RestResource;
            }
        }
    }
}