using System.Text.Json;
using JbHifi.WeatherReport.DataLibrary.Models;
using JbHifi.WeatherReport.WebApi.Attributes;
using JbHifi.WeatherReport.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace JbHifi.WeatherReport.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherReportService _weatherReportService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, 
        IWeatherReportService weatherReportService)
    {
        _logger = logger;
        _weatherReportService = weatherReportService;
    }

    [HttpGet("GetWeatherForecast")]
    [ServiceFilter(typeof(AuthorizeAttribute))]
    public async Task<ActionResult<IList<string>>> GetWeatherForecast(string city, string country)
    {
        var response = await _weatherReportService.GetWeatherForecast(city, country);
        return response.Any() ? Ok(response) : NotFound(); 
    }
}