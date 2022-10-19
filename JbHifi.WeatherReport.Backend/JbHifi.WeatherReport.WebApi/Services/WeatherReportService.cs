using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;
using Serilog.Configuration;

namespace JbHifi.WeatherReport.WebApi.Services;

public class WeatherReportService : IWeatherReportService
{
    private readonly ILogger<WeatherReportService> _logger;
    private readonly IOpenWeatherServiceApiKeyRepository _openWeatherServiceApiKeyRepository;
    private readonly IWeatherReportApiKeyRepository _weatherReportApiKeyRepository;
    
    public WeatherReportService(ILogger<WeatherReportService> logger,
        IOpenWeatherServiceApiKeyRepository openWeatherServiceApiKeyRepository,
        IWeatherReportApiKeyRepository weatherReportApiKeyRepository)
    {
        _logger = logger;
        _openWeatherServiceApiKeyRepository = openWeatherServiceApiKeyRepository;
        _weatherReportApiKeyRepository = weatherReportApiKeyRepository;
    }

    public async Task<IList<Openweatherserviceapikey>> GetAllOpenWeatherServiceApiKeys()
    {
        return await _openWeatherServiceApiKeyRepository.GetAll();
    }
     
}