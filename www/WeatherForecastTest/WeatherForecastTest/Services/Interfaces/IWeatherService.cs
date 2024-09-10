using static WeatherForecastTest.Models.WeatherForecastService;

namespace WeatherForecastTest.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<List<WeatherForecast>> GetWeatherForecastAsync(DateTime? date, string city, string country);
    }
}
