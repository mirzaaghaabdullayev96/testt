using static WeatherForecastTest.Models.WeatherForecastService;
using WeatherForecastTest.Services.Interfaces;
using Newtonsoft.Json;

namespace WeatherForecastTest.Services.Implementations
{
    public class OpenWeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "744352302a7b4089f2168f00d841a188";  // move to appsettings to make it changable.

        public OpenWeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<WeatherForecast>> GetWeatherForecastAsync(DateTime? date, string city, string country)
        {
          
            var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/forecast?q={city},{country}&appid={_apiKey}&units=metric");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var weatherData = JsonConvert.DeserializeObject<OpenWeatherResponse>(content);

           
            var forecasts = new List<WeatherForecast>();

            foreach (var item in weatherData.List.Where(data=>data.DtTxt<=date))
            {
                forecasts.Add(new WeatherForecast
                {
                    Date = item.DtTxt,
                    City = city,
                    Country = country,
                    TemperatureC = item.Main.Temp,
                    Summary = item.Weather[0].Description,
                    Source = "OpenWeatherMap"
                });
            }

            return forecasts;
        }
    }

    
    public class OpenWeatherResponse
    {
        public List<WeatherItem> List { get; set; }
    }

    public class WeatherItem
    {
        public Main Main { get; set; }
        public List<Weather> Weather { get; set; }
        [JsonProperty("dt_txt")]
        public DateTime DtTxt { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
    }

    public class Weather
    {
        public string Description { get; set; }
    }
}
