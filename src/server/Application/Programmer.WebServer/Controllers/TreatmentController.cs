using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Programmer.Common.Domain.Treatment;
using Programmer.Common.Services.Command;
using Programmer.Common.Services.Treatment;

namespace Programmer.WebServer.Controllers
{
    /// <summary>
    ///     Web Api Controller for manageing PumpInfo Resource
    /// </summary>
    [Route("api/[controller]")]
    public class TreatmentController : Controller
    {
        #region Consts

        public const string PumpSessionHeaderName = "X-Session-Token";

        #endregion


        public TreatmentController(ITreatmentService treamentService)
        {
            _treamentService = treamentService;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var srvRes = await _treamentService.GetAll();
            return srvRes.ToActionResult();
        }

        /// <summary>
        ///     Sends treatment to pump
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(TreatmentModel), (int) HttpStatusCode.Accepted)]
        [HttpPost]
        public async Task<IActionResult> CreateTreatment([FromBody] TreatmentModel treatmentModel)
        {
            var errorMessage = "";
            if (!ValidateSetTreatmentValues(treatmentModel, ref errorMessage))
                return BadRequest(new
                {
                    message = errorMessage,
                    data = treatmentModel
                });
            var srvRes = await  _treamentService.CreateTreament(treatmentModel);

            return srvRes.ToActionResult();
        }

        #region Fields

        private readonly ITreatmentService _treamentService;

        #endregion

        #region create treamtnet Utilities

        private bool ValidateSetTreatmentValues(TreatmentModel treatmentModel, ref string errorMessage)
        {
            if (treatmentModel == null)
            {
                errorMessage = "Missing treament model";
                return false;
            }

            if (Request == null || Request.Headers.IsNullOrEmpty()
                                || !Request.Headers.TryGetValue(PumpSessionHeaderName, out var sessionIdHeader)
                                || !(treatmentModel.SessionId = sessionIdHeader.ToString()).HasValue())
            {
                errorMessage = "Missing session Id";
                return false;
            }

            return true;
        }

        

        #endregion
    }
}