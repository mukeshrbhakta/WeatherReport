namespace JbHifi.WeatherReport.WebApi.Responses;

/// <summary>
/// The error response
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Correlation id for support to verify in logs
    /// </summary>
    public Guid CorrelationId { get; set; }
    
    /// <summary>
    /// Error message
    /// </summary>
    public string? ErrorMessage { get; set; }
}