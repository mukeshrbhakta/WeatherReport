namespace JbHifi.WeatherReport.Common;

/// <summary>
/// The pre-define values
/// </summary>
public static class PreDefines
{
    /// <summary>
    /// Rate limit per hour
    /// </summary>
    public const int RateLimitPerHour = 5;

    /// <summary>
    /// Default schema
    /// </summary>
    public const string DefaultSchema = "WeatherReport";
    
    /// <summary>
    /// Error message - generic
    /// </summary>
    public const string AuthoriseAttributeErrorMessage = "Missing or invalid ApiKey in header";

    /// <summary>
    /// Invalid params
    /// </summary>
    public const string InvalidParameters = "Please provide both parameters";
    
    /// <summary>
    /// Hourly limit 
    /// </summary>
    public const string RateLimitPerHourExceeded = "Your hourly limit has exceeded";

    /// <summary>
    /// Internal serialization error
    /// </summary>
    public const string SerializationProblem = "Problem serializing data";
}