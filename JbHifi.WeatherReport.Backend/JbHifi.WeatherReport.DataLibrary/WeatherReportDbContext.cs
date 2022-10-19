using Microsoft.EntityFrameworkCore;

namespace JbHifi.WeatherReport.DataLibrary.Models;

public partial class WeatherReportDbContext : DbContext
{
    /// <summary>
    /// Our connection string from AWS
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="connectionString">the connection string</param>
    public WeatherReportDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        { 
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }

}