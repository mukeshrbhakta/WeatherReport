using DevLab.JmesPath;

namespace JbHifi.WeatherReport.WebApi.Services;

public sealed class TransformService : ITransformService
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<TransformService>? _logger;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    public TransformService(ILogger<TransformService>? logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Perform transformation 
    /// </summary>
    /// <param name="request">the request payload</param>
    /// <returns>the response payload</returns>
    public async Task<string> Process(string request)
    { 
        if (string.IsNullOrEmpty(request))
        {
            return string.Empty;
        }

        // Perform transform
        return await _Process(request);  
    }

    /// <summary>
    /// Process the Jmespath query
    /// </summary>
    /// <param name="request"></param> 
    /// <returns></returns>
    private async Task<string> _Process(string request)
    {
        var response = request;
        try
        {
            var transformQuery = await GetQuery();
            
            var jmesPath = new JmesPath();
            // Perform transform
            response = jmesPath.Transform(request, transformQuery);
        }
        catch (Exception exception)
        {
            // Report the error but let's also handball the request to consumer
            _logger!.LogError($"TransformService::_Process exception {exception.Message}");
        }

        return response; 
    }

    private async Task<string> GetQuery()
    {
        return await File.ReadAllTextAsync("./Transform.query");
    }
}