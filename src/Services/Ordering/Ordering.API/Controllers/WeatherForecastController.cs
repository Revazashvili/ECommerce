using EventBus;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ordering.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IEventBus _eventBus;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,IEventBus eventBus)
    {
        _logger = logger;
        _eventBus = eventBus;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var weathers =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        foreach (var weatherForecast in weathers)
        {
            var dummyEvent = new DummyEvent
            {
                Weather = weatherForecast.Summary!
            };

            await _eventBus.PublishAsync(dummyEvent);
        }

        return weathers;
    }
}

public class DummyEvent : IntegrationEvent
{
    [JsonProperty("weather")]
    public string Weather { get; set; }
}