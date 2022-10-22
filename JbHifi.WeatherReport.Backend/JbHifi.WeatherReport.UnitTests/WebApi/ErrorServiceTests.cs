using JbHifi.WeatherReport.WebApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JbHifi.WeatherReport.UnitTests.WebApi;

[TestClass]
public class ErrorServiceTests
{
    private ILogger<ErrorService>? _logger;

    [TestInitialize]
    public void Setup()
    {
        var serviceProvider = new ServiceCollection() 
            .AddLogging()
            .BuildServiceProvider();

        _logger = serviceProvider.GetRequiredService<ILogger<ErrorService>>();
        Assert.IsNotNull(_logger);
    }
    
    [TestMethod]
    public void Constructor_Success()
    {
        // Arrange
        var errorService = new ErrorService(_logger);
        
        // Assert
        Assert.IsNotNull(errorService);
    }
    
    [TestMethod]
    public void LogException_Success()
    {
        // Arrange
        var errorService = new ErrorService(_logger);
        
        // Act 
        var result = errorService.LogException(new Exception("some error"));
        
        // Assert
        Assert.AreNotEqual(Guid.Empty, result);
    }
    
    [TestMethod]
    public void LogException_WithInnerException_Success()
    {
        // Arrange
        var errorService = new ErrorService(_logger);
        var exception = new Exception("some error", new Exception("some inner error"));
        
        // Act 
        var result = errorService.LogException(exception);
        
        // Assert
        Assert.AreNotEqual(Guid.Empty, result);
    }
}