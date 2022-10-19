using JbHifi.WeatherReport.DataLibrary.Models;

namespace JbHifi.WeatherReport.DataLibrary.Interfaces;

public interface IWeatherReportApiKeyRepository
{
    Task<IList<Weatherreportapikey>> GetAll();
    Task<Weatherreportapikey?> Get(int id);
}