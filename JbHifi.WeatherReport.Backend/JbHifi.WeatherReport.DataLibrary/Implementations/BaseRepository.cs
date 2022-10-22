using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;

namespace JbHifi.WeatherReport.DataLibrary.Implementations;

/// <summary>
/// Base class
/// </summary>
public class BaseRepository
{
    /// <summary>
    /// The db factory
    /// </summary>
    private readonly IWeatherReportDbFactory? _dbFactory;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="dbFactory">the factory handle</param>
    protected BaseRepository(IWeatherReportDbFactory? dbFactory)
    { 
        _dbFactory = dbFactory;
    } 

    /// <summary>
    /// Get the database context
    /// </summary>
    /// <returns>the db context</returns>
    protected WeatherReportDbContext GetDbContext()
    {
        return _dbFactory!.GetDbContext();
    }
}