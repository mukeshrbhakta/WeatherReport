using JbHifi.WeatherReport.DataLibrary.Models;

namespace JbHifi.WeatherReport.WebApi.Services;

public interface IWeatherReportService
{
    Task<IList<Openweatherserviceapikey>> GetAllOpenWeatherServiceApiKeys();
}