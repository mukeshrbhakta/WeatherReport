using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace JbHifi.WeatherReport.DataLibrary.Implementations;

public class AuditRepository : BaseRepository,  IAuditRepository
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="dbFactory">db factory</param>
    public AuditRepository(IWeatherReportDbFactory? dbFactory) : base(dbFactory)
    {
    }

    public async Task<IEnumerable<Audit?>> GetForWeatherReportApiKeyIdForPastHour(int id)
    {
        await using var dbContext = GetDbContext();
        var now = DateTime.UtcNow;
        var data = dbContext.Audits.Where(a => a.Weatherreportapikeysid == id 
                                               && a.Createddate >= now.AddHours(-1)
                                               && a.Createddate <= now);

        return await data.ToListAsync();
    }

    public async Task<int> Add(Audit audit)
    {
        await using var dbContext = GetDbContext();
        await dbContext.Audits.AddAsync(audit);
        await dbContext.SaveChangesAsync();
        return audit.Id;
    }
}