using System.Data;
using System.Text.Json;
using JbHifi.WeatherReport.Common;
using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;
using Serilog.Configuration;

namespace JbHifi.WeatherReport.WebApi.Services;

public class WeatherReportService : IWeatherReportService
{
    private readonly ILogger<WeatherReportService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IOpenWeatherServiceApiKeyRepository _openWeatherServiceApiKeyRepository;
    private readonly IWeatherReportApiKeyRepository _weatherReportApiKeyRepository;
    private readonly IErrorService _errorService;
    private readonly ITransformService _transformService;
    private readonly IHttpClientFactory _httpClientFactory;
    
    public WeatherReportService(ILogger<WeatherReportService> logger,
        IConfiguration configuration,
        IOpenWeatherServiceApiKeyRepository openWeatherServiceApiKeyRepository,
        IWeatherReportApiKeyRepository weatherReportApiKeyRepository, 
        IErrorService errorService,
        ITransformService transformService,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _openWeatherServiceApiKeyRepository = openWeatherServiceApiKeyRepository;
        _weatherReportApiKeyRepository = weatherReportApiKeyRepository;
        _errorService = errorService;
        _transformService = transformService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<string>> GetWeatherForecast(string city, string country)
    {
        if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(country))
        {
            throw new ApplicationException(PreDefines.InvalidParameters);
        }

        var endPoint = Helpers.GetEndpoint(_configuration);
        var data = await _openWeatherServiceApiKeyRepository.GetAll();

        foreach (var record in data)
        {
            var result = await MakeWebRequest(endPoint, record.Apikey, city, country);
            if (!string.IsNullOrWhiteSpace(result))
            {
                // Run transformation 
                var transform = await _transformService.Process(result);
                return GetJsonResponse(transform);
            }
            
            // Retry using the next API key
        }

        return new List<string>();
    }

    /// <summary>
    /// Validate api key
    /// </summary>
    /// <param name="apiKey"></param>
    public async Task ValidateApiKey(string apiKey)
    {
        var apiKeyRecords = await _weatherReportApiKeyRepository.GetAll();
        var found = apiKeyRecords.Select(record => 
            Helpers.GenerateApiKey(_configuration, record.Name, record.Uniqueid))
            .Any(generatedApiKey => apiKey == generatedApiKey);

        if (!found)
        {
            throw new UnauthorizedAccessException(PreDefines.AuthoriseAttributeErrorMessage);
        }
    }

    /// <summary>
    /// Make web request
    /// </summary>
    /// <param name="endPoint"></param>
    /// <param name="apiKey"></param>
    /// <param name="city"></param>
    /// <param name="country"></param>
    /// <returns></returns>
    private async Task<string> MakeWebRequest(
            string endPoint, string apiKey, string city, string country)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();
            return await Helpers.MakeWebRequest(
                httpClient, endPoint, apiKey, city, country);
        }
        catch (Exception exception)
        {
            _errorService.LogException(exception);
            return string.Empty;
        }
    }

    private IEnumerable<string> GetJsonResponse(string response)
    {
        if (string.IsNullOrWhiteSpace(response))
        {
            return new List<string>();
        }
 
        var json = JsonSerializer.Deserialize<List<string>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

        return json;
         
    }
}