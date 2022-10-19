using JbHifi.WeatherReport.Common;
using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;
using Microsoft.Extensions.Configuration;

namespace JbHifi.WeatherReport.DataLibrary.Implementations;

public class WeatherReportDbFactory : IWeatherReportDbFactory
{
    /// <summary>
    /// The connection string 
    /// </summary>
    private readonly string _connectionString; 

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="configuration">config</param>
    public WeatherReportDbFactory(IConfiguration configuration)
    { 
        var connectionString = Helpers.GetDatabaseConnectionString(configuration);
        _connectionString = connectionString;
    }

    /// <summary>
    /// Get the db context
    /// </summary>
    /// <returns>the connection context</returns>
    public WeatherReportDbContext GetDbContext()
    { 
        return new WeatherReportDbContext(_connectionString);
    }
}