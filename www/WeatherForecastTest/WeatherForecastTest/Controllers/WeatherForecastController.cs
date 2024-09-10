using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WeatherForecastTest.Services.Implementations;
using WeatherForecastTest.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static WeatherForecastTest.Models.WeatherForecastService;

namespace WeatherForecastTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IEnumerable<IWeatherService> _weatherServices;

        public WeatherForecastController(IEnumerable<IWeatherService> weatherServices)
        {
            _weatherServices = weatherServices;
        }


        [HttpGet("{date}")]
        public async Task<IActionResult> GetWeatherForecast([FromRoute][Required] DateTime? date, [Required][FromQuery] string city, [Required][FromQuery] string country)
        {
            int daysDifference = (date.Value - DateTime.Today).Days;

            if (daysDifference > 3 || daysDifference < 0) return BadRequest("The weather forecast is available only for today and the next 3 days.");

            var allForecasts = new List<WeatherForecast>();
            var errors = new List<string>();

            foreach (var service in _weatherServices)
            {
                try
                {
                    var forecasts = await service.GetWeatherForecastAsync(date, city, country);
                    allForecasts.AddRange(forecasts);
                }
                catch (Exception ex)
                {
                    errors.Add($"Error from service {service.GetType().Name}: {ex.Message}");
                }
            }

            if (allForecasts.Count == 0)
            {
                return NotFound("No weather forecast could be found for the specified city or country. " +
                                "Errors from services: " + string.Join("; ", errors));
            }

            return Ok(allForecasts);
        }
    }
}
