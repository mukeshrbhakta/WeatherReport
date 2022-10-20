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

    public const string InvalidParameters = "Please provide both parameters";
    
    public const string InvalidTransformParameters = "Transform parameters are invalid";

}