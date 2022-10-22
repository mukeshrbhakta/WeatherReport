using System.Text.Json;
using JbHifi.WeatherReport.WebApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JbHifi.WeatherReport.UnitTests.WebApi;

[TestClass]
public class TransformServiceTests
{
    private ILogger<TransformService>? _logger;

    [TestInitialize]
    public void Setup()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        _logger = serviceProvider.GetRequiredService<ILogger<TransformService>>();
        Assert.IsNotNull(_logger);
    }

    [TestMethod]
    public void Constructor_Success()
    {
        // Arrange
        var service = new TransformService(_logger);

        // Assert
        Assert.IsNotNull(service);
    }

    [TestMethod]
    public async Task Process_Success()
    {
        // Arrange
        var service = new TransformService(_logger);
        var json = GetTestJson();

        // Act 
        var response = await service.Process(json);
        
        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(response));
        Assert.AreEqual("[\"some description\"]", response);
    }
    
    [TestMethod]
    public async Task Process_DodgyInput_Fail()
    {
        // Arrange
        var service = new TransformService(_logger);
        var json = GetDodgyJson();

        // Act 
        var response = await service.Process(json);
        
        // Assert
        Assert.IsTrue(string.IsNullOrWhiteSpace(response));
    }
    
    [TestMethod]
    public async Task Process_EmptyInput_Fail()
    {
        // Arrange
        var service = new TransformService(_logger); 

        // Act 
        var response = await service.Process(string.Empty);
        
        // Assert
        Assert.IsTrue(string.IsNullOrWhiteSpace(response));
    }

    private string GetTestJson()
    {
        var obj = new List<dynamic>()
        {
            new
            {
                weather = new List<dynamic>()
                {
                    new
                    {
                        description = "some description",
                        icon = "smiley"
                    }
                }
            }
        };

        return JsonSerializer.Serialize(obj);
    }
    
    private string GetDodgyJson()
    {
        var obj = new
        {
            Address = "downing street",
            City = "london"
        };

        return JsonSerializer.Serialize(obj);
    }
}