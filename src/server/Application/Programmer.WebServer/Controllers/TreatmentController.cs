using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Programmer.Common.Services.Treatment;

namespace Programmer.WebServer.Controllers
{
    /// <summary>
    /// Web Api Controller for manageing PumpInfo Resource
    /// </summary>
    [Route("api/[controller]")]
    public class TreatmentController :Controller
    {
        private readonly ITreatmentService _treamentService;

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
    }
}
