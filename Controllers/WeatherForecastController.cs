using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService _service;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger , IWeatherForecastService service )
        {
            _logger = logger;
            _service = service;
        }

        // optional  [HttpPost][Route("generate/{number}")] 
        [HttpPost("generate/{number}")]
        public ActionResult<IEnumerable<WeatherForecast>> Generate([FromBody] int max, [FromBody] int min, [FromRoute] int number)
        {

            if(number < 0 || max < min )
            {
                return BadRequest();
            }

            //option 1
            //HttpContext.Response.StatusCode = 401;

            //option 2
            //return StatusCode(401, ("Hello " + name));

            var result = _service.Get(min:min, max:max, number:number);

            return OK(result);

        }

    }
}
