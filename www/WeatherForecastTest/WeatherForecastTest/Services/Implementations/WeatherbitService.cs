using Newtonsoft.Json;
using static WeatherForecastTest.Models.WeatherForecastService;
using WeatherForecastTest.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WeatherForecastTest.Services.Implementations
{
    public class WeatherbitService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "14adebbf61fb4f3db604e05f8d989a02";

        public WeatherbitService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<WeatherForecast>> GetWeatherForecastAsync(DateTime? date, string city, string country)
        {

            var response = await _httpClient.GetAsync($"https://api.weatherbit.io/v2.0/forecast/daily?city={city},{country}&key={_apiKey}");

           
            //buna baxmaq
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var weatherData = JsonConvert.DeserializeObject<WeatherbitResponse>(content);


            var forecasts = new List<WeatherForecast>();

            foreach (var item in weatherData.Data.Where(data=>data.Datetime<=date))
            {
                forecasts.Add(new WeatherForecast
                {
                    Date = item.Datetime,
                    City = city,
                    Country = country,
                    TemperatureC = item.Temp,
                    Summary = item.Weather.Description,
                    Source = "Weatherbit"
                });
            }

            return forecasts;
        }
    }


    public class WeatherbitResponse
    {
        public List<WeatherbitData> Data { get; set; }
    }

    public class WeatherbitData
    {
        public DateTime Datetime { get; set; }
        public double Temp { get; set; }
        public WeatherbitWeather Weather { get; set; }
    }

    public class WeatherbitWeather
    {
        public string Description { get; set; }
    }
}
