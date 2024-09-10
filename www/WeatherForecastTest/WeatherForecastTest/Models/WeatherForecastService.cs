namespace WeatherForecastTest.Models
{
    public class WeatherForecastService
    {
        public class WeatherForecast
        {
            public DateTime Date { get; set; }          
            public string City { get; set; }            
            public string Country { get; set; }         
            public double TemperatureC { get; set; }    
            public string Summary { get; set; }         
            public string Source { get; set; }          
        }
    }
}
