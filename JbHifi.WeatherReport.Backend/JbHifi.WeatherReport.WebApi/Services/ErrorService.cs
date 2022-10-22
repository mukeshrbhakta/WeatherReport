using System.Diagnostics;

namespace JbHifi.WeatherReport.WebApi.Services;

public sealed class ErrorService : IErrorService
{
    private readonly ILogger<ErrorService> _logger;

    public ErrorService(ILogger<ErrorService> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Exception handler
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public Guid LogException(Exception exception)
    {
        var correlationId = Guid.NewGuid();
        _logger.LogError($"Correlation id {correlationId}\tMessage {exception}");
        
        if (exception.InnerException != null)
        {
            _logger.LogError($"Correlation id {correlationId}\tInnerException {exception.InnerException}");
        }
        
        LogStackTrace(correlationId, exception);

        return correlationId;
    }

    /// <summary>
    /// Stack trace for debugging
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="exception"></param>
    [Conditional("DEBUG")]
    private void LogStackTrace(Guid correlationId, Exception exception)
    {
        if (!string.IsNullOrWhiteSpace(exception.StackTrace))
        {
            _logger.LogError($"Correlation id {correlationId}\tStack trace -\r\n{exception.StackTrace}");
        }
    }
}