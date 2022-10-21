using JbHifi.WeatherReport.DataLibrary.Models;

namespace JbHifi.WeatherReport.DataLibrary.Interfaces;

public interface IOpenWeatherServiceApiKeyRepository
{
    Task<IList<Openweatherserviceapikey>> GetAll();
    Task<Openweatherserviceapikey?> Get(int id);
}