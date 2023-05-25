using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Threading;

namespace RestaurantApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService _service;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]                         //endpoint domyœlny o nazwie Controlera (weatherForecast)
        public IEnumerable<WeatherForecast> Get()
        {
            var result = _service.Get();
            return result;
        }

        [HttpGet("currentDay/{max}")]                         //dodanie endpointu wer. I  (weatherForecast/currentDay)
        public IEnumerable<WeatherForecast> Get2([FromQuery]int take, [FromRoute]int max) //dodawanie parametrów
        {
            var result = _service.Get();
            return result;
        }

        [HttpGet]
        [Route("currentDay2")]                         //dodanie endpointu wer. II (weatherForecast/currentDay2)
        public IEnumerable<WeatherForecast> Get3()
        {
            var result = _service.Get();
            return result;
        }

        [HttpPost("sayHello")]
        public string Hello([FromBody] string name)
        {
            return $"Hello {name}";
        }

        [HttpPost]
        public ActionResult<string> Hello2([FromBody] string name)
        {
            //HttpContext.Response.StatusCode = 401;                  //pierwszy sposób na przes³anie kodu zapytania
            //return $"Hello {name}";

            return NotFound($"Hello {name}");                       //drugi sposób na przes³anie kodu zapytania
        }

        [HttpGet("zadanie")]         //sposób I                
        public IEnumerable<WeatherForecast> Get3([FromQuery] int resultsNumber, [FromQuery] int maxGrad, [FromQuery] int minGrad) 
        {
            var result = _service.Get(resultsNumber,  maxGrad,  minGrad);
            return result;
        }

        [HttpGet("zadanie2/{resultsNumber}/{maxGrad}/{minGrad}")]       //sposób II
        public IEnumerable<WeatherForecast> Get4([FromRoute] int resultsNumber, [FromRoute] int maxGrad, [FromRoute] int minGrad)
        {
            var result = _service.Get(resultsNumber, maxGrad, minGrad);
            return result;
        }

        [HttpPost("generate")]
        public ActionResult<IEnumerable<WeatherForecast>> temperature([FromQuery] int resultsNumber, 
            [FromBody] TemperatureRequest request)
        {
            if(resultsNumber < 0 || request.maxGrad<request.minGrad) return BadRequest();
            var result = _service.Get(resultsNumber, request.minGrad, request.maxGrad);
            return Ok(result);                       
        }
    }
}