using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Programmer.Common.Domain.Treatment;
using Programmer.Common.Services.Command;

namespace Programmer.WebServer.Controllers
{
    /// <summary>
    /// Web Api Controller for manageing PumpInfo Resource
    /// </summary>
    [Route("api/[controller]")]
    public class CommandController : Controller
    {
        #region Consts
        public const string PumpSessionHeaderName = "X-Session-Token";
        #endregion
        #region Fields
        private readonly ICommandService _commandService;

        #endregion

        #region CTOR
        public CommandController(ICommandService commandService)
        {
            _commandService = commandService;
        }

        #endregion

        /// <summary>
        /// Sends treatment to pump
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(TreatmentModel), (int)HttpStatusCode.Accepted)]
        [HttpPost("treatment")]
        public async Task<IActionResult> SetTreatment([FromBody] TreatmentModel treatmentModel)
        {
            var errorMessage = "";
            if (!ValidateSetTreatmentValues(treatmentModel, ref errorMessage))
            {
                return BadRequest(new
                {
                    message = errorMessage,
                    data = treatmentModel
                });
            }
            var cmdRequest = ToCommandRequest("treatment", treatmentModel.SessionId, treatmentModel);

            var srvRes = await _commandService.SendCommand(cmdRequest);

            return srvRes.ToActionResult();
        }

        private CommandRequest ToCommandRequest<T>(string cmdName, string sessionId, T treatmentModel)
        {
            var cmdReq = new CommandRequest(cmdName, sessionId);
            var pInfos = typeof(T).GetProperties();

            foreach (var pi in pInfos)
                cmdReq.Parameters[pi.Name] = pi.GetValue(treatmentModel);

            return cmdReq;
        }

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
    }
}
