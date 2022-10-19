using Microsoft.Extensions.Configuration;

namespace JbHifi.WeatherReport.Common;

public static class Helpers
{
    /// <summary>
    /// Get the db connection string
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static string GetDatabaseConnectionString(IConfiguration? configuration)
    {
        var key = configuration!["Key"]; 
        var connStr = configuration["ConnectionString"];
        
        return Security.Decrypt(key, connStr);
    }
}