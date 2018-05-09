using Microsoft.AspNetCore.Mvc;

namespace Calculator.WebServer.Controllers
{
    [Route("api/[controller]")]
    public class RatesController : Controller
    {
        private const decimal DefaultDecimal = default(decimal);
        // GET api/values
        [HttpGet]
        public IActionResult GetRates(decimal rate, decimal dose, decimal vtbi)
        {
            if (MissingData(rate, dose, vtbi))
            {
                return new BadRequestObjectResult(new
                {
                    vtbi,
                    rate,
                    dose,
                    message = "2 of 3 fields are mandatory"
                });
            }

            Calculate(ref rate, ref dose, ref vtbi);
            return Ok(new
            {
                vtbi,
                rate,
                dose,
            });
        }

        private void Calculate(ref decimal rate, ref decimal dose, ref decimal vtbi)
        {
            if (rate > DefaultDecimal && dose > DefaultDecimal)
            {
                vtbi = rate * dose;
                return;
            }

            if (rate > DefaultDecimal && vtbi > DefaultDecimal)
            {
                dose = vtbi / rate;
                return;
            }

             rate = vtbi / dose;
        }

        private static bool MissingData(decimal rate, decimal dose, decimal vtbi)
        {

            return (rate == DefaultDecimal && dose == DefaultDecimal && vtbi == DefaultDecimal)
                   || (rate == DefaultDecimal && dose == DefaultDecimal)
                   || (rate == DefaultDecimal && vtbi == DefaultDecimal)
                   || (dose == DefaultDecimal && vtbi == DefaultDecimal);

        }
    }
}
