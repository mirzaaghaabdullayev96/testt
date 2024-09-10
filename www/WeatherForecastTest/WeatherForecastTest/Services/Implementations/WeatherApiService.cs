using Newtonsoft.Json;
using static WeatherForecastTest.Models.WeatherForecastService;
using WeatherForecastTest.Services.Interfaces;

namespace WeatherForecastTest.Services.Implementations
{
    public class WeatherApiService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "cabffcc91d524627ad6180004240909";

        public WeatherApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<WeatherForecast>> GetWeatherForecastAsync(DateTime? date, string city, string country)
        {


            var response = await _httpClient.GetAsync($"http://api.weatherapi.com/v1/forecast.json?key={_apiKey}&q={city},{country}&days=4");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var weatherData = JsonConvert.DeserializeObject<WeatherApiResponse>(content);



            var forecasts = new List<WeatherForecast>();

            foreach (var item in weatherData.Forecast.Forecastday.Where(x => x.Date <= date))
            {
                foreach (var hour in item.Hour)
                {
                    forecasts.Add(new WeatherForecast
                    {
                        Date = hour.Time,
                        City = city,
                        Country = country,
                        TemperatureC = hour.TempC,
                        Summary = hour.Condition.Text,
                        Source = "WeatherAPI"
                    });
                }

            }

            return forecasts;
        }
    }

    public class WeatherApiResponse
    {
        public Forecast Forecast { get; set; }
    }

    public class Forecast
    {
        public List<Forecastday> Forecastday { get; set; }
    }

    public class Forecastday
    {
        public DateTime Date { get; set; }
        public List<Hour> Hour { get; set; }
    }

    public class Hour
    {
        [JsonProperty("temp_c")]
        public double TempC { get; set; }
        public Condition Condition { get; set; }
        public DateTime Time { get; set; }
    }

    public class Condition
    {
        public string Text { get; set; }
    }
}
