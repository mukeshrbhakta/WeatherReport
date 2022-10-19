using JbHifi.WeatherReport.DataLibrary.Models;
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

    [HttpGet("GetAllOpenWeatherServiceApiKeys")]
    public async Task<ActionResult<IList<Openweatherserviceapikey>>> GetAllOpenWeatherServiceApiKeys()
    {
        var response = await _weatherReportService.GetAllOpenWeatherServiceApiKeys();
        return Ok(response);
    }
}