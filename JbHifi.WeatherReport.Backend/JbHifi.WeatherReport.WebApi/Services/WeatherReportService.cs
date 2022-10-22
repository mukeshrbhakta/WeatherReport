using System.Text.Json;
using JbHifi.WeatherReport.Common;
using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;

namespace JbHifi.WeatherReport.WebApi.Services;

/// <summary>
/// The main service
/// </summary>
public class WeatherReportService : IWeatherReportService
{ 
    private readonly IConfiguration _configuration;
    private readonly IOpenWeatherServiceApiKeyRepository _openWeatherServiceApiKeyRepository;
    private readonly IWeatherReportApiKeyRepository _weatherReportApiKeyRepository;
    private readonly IAuditRepository _auditRepository;
    private readonly IErrorService _errorService;
    private readonly ITransformService _transformService;
    private readonly IHttpClientFactory _httpClientFactory;
    
    public WeatherReportService(
        IConfiguration configuration,
        IOpenWeatherServiceApiKeyRepository openWeatherServiceApiKeyRepository,
        IWeatherReportApiKeyRepository weatherReportApiKeyRepository,
        IAuditRepository auditRepository,
        IErrorService errorService,
        ITransformService transformService,
        IHttpClientFactory httpClientFactory)
    { 
        _configuration = configuration;
        _openWeatherServiceApiKeyRepository = openWeatherServiceApiKeyRepository;
        _weatherReportApiKeyRepository = weatherReportApiKeyRepository;
        _auditRepository = auditRepository;
        _errorService = errorService;
        _transformService = transformService;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Get the forecast for a city and country
    /// </summary>
    /// <param name="city"></param>
    /// <param name="country"></param>
    /// <returns></returns>
    public async Task<IEnumerable<string>> GetWeatherForecast(string city, string country)
    {
        Validate(city, country);

        var endPoint = Helpers.GetEndpoint(_configuration);
        var data = await _openWeatherServiceApiKeyRepository.GetAll();

        var dataList = new List<string>();

        // Parse through services
        foreach (var record in data)
        {
            var result = await MakeWebRequest(endPoint, record.Apikey, city, country);
            // filter out invalids and duplicates
            if (!string.IsNullOrWhiteSpace(result) && !dataList.Contains(result))
            {
                dataList.Add(result);
            } 
        }
        
        // Run transformation 
        var transform = await _transformService.Process(dataList.ToJsonArray());
        return GetJsonResponse(transform);
    }
 
    /// <summary>
    /// Validate api key
    /// </summary>
    /// <param name="apiKey"></param>
    public async Task ValidateApiKey(string apiKey)
    {
        var apiKeyRecords = await _weatherReportApiKeyRepository.GetAll();
        var apiKeyId = 0;
        var rateLimitPerHour = 0;
        
        foreach (var record in apiKeyRecords)
        {
            var generatedKey = Helpers.GenerateApiKey(record.Name, record.Uniqueid);
            if (generatedKey == apiKey)
            {
                apiKeyId = record.Id;
                rateLimitPerHour = record.Ratelimitperhour;
                break;
            }
        }
        
        if (apiKeyId == 0) // not found 
        {
            throw new UnauthorizedAccessException(PreDefines.AuthoriseAttributeErrorMessage);
        }

        var withinLimits = await WithinHourlyLimit(apiKeyId, rateLimitPerHour);

        if (!withinLimits) // Above limit
        {
            throw new BadHttpRequestException(PreDefines.RateLimitPerHourExceeded);
        }
        
        // Success
        await _auditRepository.Add(new Audit()
        {
            Weatherreportapikeysid = apiKeyId
        });
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

    /// <summary>
    /// Check if we are within the limits 
    /// </summary>
    /// <param name="apiKeyId"></param>
    /// <param name="rateLimitPerHour"></param>
    /// <returns></returns>
    private async Task<bool> WithinHourlyLimit(int apiKeyId, int rateLimitPerHour)
    {
        var records = await _auditRepository.GetForWeatherReportApiKeyIdForPastHour(apiKeyId);
        return records.Count() < rateLimitPerHour;
    }

    /// <summary>
    /// Convert string to json response
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
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

        if (json == null)
        {
            throw new InvalidOperationException(PreDefines.SerializationProblem);
        }
        
        return json;
    }
    
    /// <summary>
    /// Validate the inputs
    /// </summary>
    /// <param name="city"></param>
    /// <param name="country"></param>
    /// <exception cref="ArgumentException"></exception>
    private static void Validate(string city, string country)
    {
        if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException(PreDefines.InvalidParameters);
        }
    }

}