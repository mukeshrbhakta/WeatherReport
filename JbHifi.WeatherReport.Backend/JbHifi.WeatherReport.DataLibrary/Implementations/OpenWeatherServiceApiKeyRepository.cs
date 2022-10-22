using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;

namespace JbHifi.WeatherReport.DataLibrary.Implementations;

/// <summary>
/// OpenWeatherServiceApiKey repo
/// </summary>
public sealed class OpenWeatherServiceApiKeyRepository : BaseRepository, IOpenWeatherServiceApiKeyRepository
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="dbFactory">db factory</param>
    public OpenWeatherServiceApiKeyRepository(IWeatherReportDbFactory? dbFactory) : base(dbFactory)
    {
    }
    
    /// <summary>
    /// Get all records
    /// </summary>
    /// <returns></returns>
    public async Task<IList<Openweatherserviceapikey>> GetAll()
    {
        await using var dbContext = GetDbContext(); 
        return dbContext.Openweatherserviceapikeys.ToList();
    }

    /// <summary>
    /// Get a record
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Openweatherserviceapikey?> Get(int id)
    {
        await using var dbContext = GetDbContext();
        return dbContext.Openweatherserviceapikeys
            .FirstOrDefault(a => a.Id == id);
    }
}