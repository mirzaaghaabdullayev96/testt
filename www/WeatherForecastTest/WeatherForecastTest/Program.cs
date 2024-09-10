
using WeatherForecastTest.Services.Implementations;
using WeatherForecastTest.Services.Interfaces;

namespace WeatherForecastTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient<IWeatherService,OpenWeatherService>();
            builder.Services.AddHttpClient<IWeatherService,WeatherApiService>();
            builder.Services.AddHttpClient<IWeatherService,WeatherbitService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
