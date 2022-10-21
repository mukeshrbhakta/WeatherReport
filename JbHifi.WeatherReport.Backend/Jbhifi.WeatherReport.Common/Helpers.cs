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

    /// <summary>
    /// Generate Api key
    /// </summary>
    /// <param name="name"></param>
    /// <param name="uniqueId"></param>
    /// <returns></returns>
    public static string GenerateApiKey(string name, Guid uniqueId)
    {
        var magicString = $"{name}::{uniqueId}";
        return Security.GetHash(magicString);
    }

    /// <summary>
    /// Get the end point
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static string GetEndpoint(IConfiguration? configuration)
    {
        var key = configuration!["Key"];
        var endPoint = configuration!["Endpoint"];
        return Security.Decrypt(key, endPoint);
    }

    /// <summary>
    /// Make web request
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="endPoint"></param>
    /// <param name="apiKey"></param>
    /// <param name="city"></param>
    /// <param name="country"></param>
    /// <returns></returns>
    public static async Task<string> MakeWebRequest(HttpClient httpClient,
        string endPoint,
        string apiKey, string city, string country)
    {
        var uri = new Uri($"{endPoint}?q={city},{country}&appid={apiKey}");
        using var response = await httpClient.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync(); 
        }
         
        return string.Empty;
    }
}