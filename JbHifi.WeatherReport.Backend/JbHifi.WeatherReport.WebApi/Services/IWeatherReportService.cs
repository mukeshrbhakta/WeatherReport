namespace JbHifi.WeatherReport.WebApi.Services;

public interface IWeatherReportService
{
    Task<IEnumerable<string>> GetWeatherForecast(string city, string country);
    Task ValidateApiKey(string apiKey);
}