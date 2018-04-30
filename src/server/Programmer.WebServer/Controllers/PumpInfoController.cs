using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Programmer.Common.Domain.Pump;
using Programmer.Common.Services.Pump;

namespace Programmer.WebServer.Controllers
{
    /// <summary>
    /// Web Api Controller for manageing PumpInfo Resource
    /// </summary>
    [Route("api/[controller]")]
    public class PumpInfoController : Controller
    {
        #region Fields
        private readonly IPumpInfoService _pumpInfoService;

        #endregion

        #region CTOR

        /// <summary>
        /// PumpInfoController constructor
        /// </summary>
        /// <param name="pumpInfoService"></param>
        public PumpInfoController(IPumpInfoService pumpInfoService)
        {
            _pumpInfoService = pumpInfoService;
        }

        #endregion

        /// <summary>
        /// Gets pump info by Id
        /// </summary>
        /// <param name="pumpInfoId">pump info id</param>
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(PumpInfoModel), (int)HttpStatusCode.OK)]
        [HttpGet("{pumpInfoId}")]
        public async Task<IActionResult> GetById(string pumpInfoId)
        {
            if (!pumpInfoId.HasValue())
                return BadRequest(new
                {
                    ErrorMessage = "Missing or illegal id",
                    Id = pumpInfoId
                });
            var srvRes = await _pumpInfoService.GetPumpInfoById(pumpInfoId);
            return srvRes.ToActionResult();
        }
    }
}
