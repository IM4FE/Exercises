using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Services;
using test;

namespace SwaggerWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StringConversionController : ControllerBase
    {
        private readonly ILogger<StringConversionController> _logger;

        public StringConversionController(ILogger<StringConversionController> logger)
        {
            _logger = logger;
        }
        [HttpGet(Name = "GetWeatherForecast")]
        public StringConversion Get(
            [FromQuery(Name = "string")] string str) => new StringConversion
        {
            str = str,
                result = WorkWithString.Work(str).Result
            };
    }
}