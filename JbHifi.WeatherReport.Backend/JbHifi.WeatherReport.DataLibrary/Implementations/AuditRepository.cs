using JbHifi.WeatherReport.Common;
using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;

namespace JbHifi.WeatherReport.DataLibrary.Implementations;

/// <summary>
/// Audit repo
/// </summary>
public sealed class AuditRepository : BaseRepository,  IAuditRepository
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="dbFactory">db factory</param>
    public AuditRepository(IWeatherReportDbFactory? dbFactory) : base(dbFactory)
    {
    }

    /// <summary>
    /// Get records for the past hour
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Audit?>> GetForWeatherReportApiKeyIdForPastHour(int id)
    {
        await using var dbContext = GetDbContext();
        var now = DateTime.UtcNow;
        var data = dbContext.Audits.Where(a => a != null
                                               && a.Weatherreportapikeysid == id 
                                               && a.Createddate >= now.AddHours(-1) 
                                               && a.Createddate <= now);

        return data.ToList();
    }

    /// <summary>
    /// Add new record
    /// </summary>
    /// <param name="audit"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public async Task<int> Add(Audit audit)
    {
        if (audit == null)
        {
            throw new ArgumentNullException(nameof(audit));
        }

        await using var dbContext = GetDbContext();
        await dbContext.Audits.AddAsync(audit);
        await dbContext.SaveChangesAsync();
        return audit.Id;
    }
}