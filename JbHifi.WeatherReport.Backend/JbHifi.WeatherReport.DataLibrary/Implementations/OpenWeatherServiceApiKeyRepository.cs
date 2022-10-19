using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace JbHifi.WeatherReport.DataLibrary.Implementations;

public class OpenWeatherServiceApiKeyRepository : BaseRepository, IOpenWeatherServiceApiKeyRepository
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="dbFactory">db factory</param>
    public OpenWeatherServiceApiKeyRepository(IWeatherReportDbFactory? dbFactory) : base(dbFactory)
    {
    }
    
    public async Task<IList<Openweatherserviceapikey>> GetAll()
    {
        await using var dbContext = GetDbContext();
        var data = dbContext.Openweatherserviceapikeys;

        if (data == null)
        {
            return new List<Openweatherserviceapikey>();
        }

        return await data.ToListAsync();
    }

    public async Task<Openweatherserviceapikey?> Get(int id)
    {
        await using var dbContext = GetDbContext();
        return await dbContext.Openweatherserviceapikeys
            .Where(a => a != null && a.Id == id)
            .FirstOrDefaultAsync();
    }
}