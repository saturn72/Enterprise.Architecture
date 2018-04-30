using System;
using System.Threading.Tasks;
using CacheManager;
using Programmer.Common.Domain.Pump;

namespace Programmer.Common.Services.Pump
{
    public class PumpInfoService : IPumpInfoService
    {
        #region consts
        protected const string RestResource = "api/pumpinfo";
        private const string PumpInfoByIdCacheKeyFormat = "pumpinfo.id:{0}";
        private const uint PumpInfoCacheTimeInSeconds = 60;

        #endregion 

        #region fields
        private readonly ICacheManager _cacheManager;
        private readonly IPumpInfoRepository _pumpInfoRepository;

        #endregion

        #region ctor
        public PumpInfoService(ICacheManager cacheManager, IPumpInfoRepository pumpInfoRepository)
        {
            _cacheManager = cacheManager;
            _pumpInfoRepository = pumpInfoRepository;
        }

        #endregion
        public async Task<ServiceResponse<PumpInfoModel>> GetPumpInfoById(string pumpId)
        {
            var srvResponse = new ServiceResponse<PumpInfoModel>();
            if (!pumpId.HasValue() || pumpId.Contains(" "))
            {
                srvResponse.Result = ServiceResponseResult.BadOrMissingData;
                srvResponse.ErrorMessage = "pump id in wrong format";
                return srvResponse;
            }
            return await Task.Run(() =>
            {
                var model = _cacheManager.Get(
                    string.Format(PumpInfoByIdCacheKeyFormat, pumpId),
                    () => _pumpInfoRepository.GetById(pumpId).Result,
                    PumpInfoCacheTimeInSeconds);

                if (model == null)
                {
                    srvResponse.Result = ServiceResponseResult.NotFound;
                }
                else
                {
                    srvResponse.Result = ServiceResponseResult.Success;
                    srvResponse.Data = model;
                }
                return srvResponse;
            });
        }
    }
}