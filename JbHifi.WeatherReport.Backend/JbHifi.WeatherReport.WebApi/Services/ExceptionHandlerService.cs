using System.Diagnostics;
using System.Net;
using System.Text.Json;
using JbHifi.WeatherReport.WebApi.Responses;

namespace JbHifi.WeatherReport.WebApi.Services;

/// <summary>
/// The global error handler
/// </summary>
public class ExceptionHandlerService
{ 
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerService> _logger;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public ExceptionHandlerService(RequestDelegate next, ILogger<ExceptionHandlerService> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    /// <summary>
    /// The main loop
    /// </summary>
    /// <param name="context"></param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var correlationId = LogException(ex);
            await HandleExceptionAsync(context, ex, correlationId);
        }
    }

    /// <summary>
    /// Exception handler
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    private Guid LogException(Exception exception)
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

    [Conditional("DEBUG")]
    private void LogStackTrace(Guid correlationId, Exception exception)
    {
        if (!string.IsNullOrWhiteSpace(exception.StackTrace))
        {
            _logger.LogError($"Correlation id {correlationId}\tStack trace -\r\n{exception.StackTrace}");
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, Guid correlationId)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        var errorResponse = new ErrorResponse()
        {
            CorrelationId = correlationId
        };

        switch (exception)
        {
            case ApplicationException: // UI display exception 
                response.StatusCode = (int) HttpStatusCode.BadRequest;
                errorResponse.ErrorMessage = exception.Message;
                break;
            default:
                // all Exception sink
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                errorResponse.ErrorMessage = $"Please contact support quoting the correlation id - {correlationId}";
                break;
        }

        var result = JsonSerializer.Serialize(errorResponse);
        await response.WriteAsync(result);
    }
}