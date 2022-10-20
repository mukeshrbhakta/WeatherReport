using System.Net;
using JbHifi.WeatherReport.Common;
using JbHifi.WeatherReport.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JbHifi.WeatherReport.WebApi.Attributes;

public class AuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
{
    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<AuthorizeAttribute>? _logger;

    /// <summary>
    /// Service
    /// </summary>
    private readonly IWeatherReportService _weatherReportService;
    private readonly IErrorService _errorService;
 
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="weatherReportService"></param>
    /// <param name="errorService"></param>
    public AuthorizeAttribute(ILogger<AuthorizeAttribute>? logger, 
        IWeatherReportService weatherReportService, 
        IErrorService errorService)
    {
        _logger = logger;
        _weatherReportService = weatherReportService;
        _errorService = errorService;
    }
    
    /// <summary>
    /// Event handler
    /// </summary>
    /// <param name="context"></param>
    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            await ValidateHeaders(context);
        }
        catch (UnauthorizedAccessException exception)
        {
            context.Result = new ObjectResult(exception.Message)
            {
                StatusCode = (int) HttpStatusCode.Unauthorized
            };
        }
        catch (Exception exception)
        {
            var correlationId =  _errorService.LogException(exception);
            var errorMessage = $"Please contact support quoting the correlation id - {correlationId}";

            context.Result = new ObjectResult(errorMessage)
            {
                StatusCode = (int) HttpStatusCode.InternalServerError
            };
        }
    }
    
    /// <summary>
    /// Validate the Api user against the domain
    /// </summary>
    /// <param name="context">the context</param>
    /// <returns>the Api user in the format CL likes</returns>
    private async Task ValidateHeaders(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;
        var headers = request.Headers;

        var apiKey =  headers["API-KEY"];
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new UnauthorizedAccessException(PreDefines.AuthoriseAttributeErrorMessage);
        }

        await _weatherReportService.ValidateApiKey(apiKey);
    }
}