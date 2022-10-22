using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;

namespace JbHifi.WeatherReport.DataLibrary.Implementations;

/// <summary>
/// WeatherReportApiKey repo
/// </summary>
public sealed class WeatherReportApiKeyRepository : BaseRepository, IWeatherReportApiKeyRepository
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="dbFactory">db factory</param>
    public WeatherReportApiKeyRepository(IWeatherReportDbFactory? dbFactory) : base(dbFactory)
    {
    }
   
    /// <summary>
    /// Get all records 
    /// </summary>
    /// <returns></returns>
    public async Task<IList<Weatherreportapikey>> GetAll()
    {
        await using var dbContext = GetDbContext();
        return dbContext.Weatherreportapikeys.ToList();
    }

    /// <summary>
    /// Get a record
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Weatherreportapikey?> Get(int id)
    {
        await using var dbContext = GetDbContext();
        return dbContext.Weatherreportapikeys
            .FirstOrDefault(a => a.Id == id);
    }
     
}