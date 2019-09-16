using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace loggingdemo.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private ILogger<SampleDataController> _logger;

        public SampleDataController(ILogger<SampleDataController> logger) => _logger = logger;

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<WeatherForecast>> TwoHundred()
        {
            _logger.LogDebug("WeatherForecasts was called");

            var rng = new Random();
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });

            return Ok(forecasts);
        }

        [HttpGet("[action]")]
        public ActionResult FiveHundred()
        {
            var rng = new Random();

            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });

            try
            {
                _logger.LogError("ERROR! {@WeatherForecasts}", forecasts);
                throw new ApplicationException("Error here!");
            }
            catch (Exception e)
            {
                _logger.LogError("Error Caught!, {@Exception}", e);
            }
            return StatusCode(500, "BOOM!");
        }

        [HttpGet("[action]")]
        public ActionResult FourHundred()
        {
            var rng = new Random();

            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });

            _logger.LogWarning("{@traceId} : Warning! {@WeatherForecasts}", HttpContext.TraceIdentifier, forecasts);
            return BadRequest(new { error = "Bad Request!", forecasts });
        }

        public class WeatherForecast
        {
            public DateTime Date { get; set; }
            public string DateFormatted => Date.ToString("d");
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}