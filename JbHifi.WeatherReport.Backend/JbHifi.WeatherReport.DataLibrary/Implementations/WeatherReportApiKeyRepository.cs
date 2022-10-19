using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace JbHifi.WeatherReport.DataLibrary.Implementations;

public class WeatherReportApiKeyRepository : BaseRepository, IWeatherReportApiKeyRepository
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="dbFactory">db factory</param>
    public WeatherReportApiKeyRepository(IWeatherReportDbFactory? dbFactory) : base(dbFactory)
    {
    }
    
    public async Task<IList<Weatherreportapikey>> GetAll()
    {
        await using var dbContext = GetDbContext();
        var data = dbContext.Weatherreportapikeys;

        if (data == null)
        {
            return new List<Weatherreportapikey>();
        }

        return await data.ToListAsync();
    }

    public async Task<Weatherreportapikey?> Get(int id)
    {
        await using var dbContext = GetDbContext();
        return await dbContext.Weatherreportapikeys
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();
    }
     
}