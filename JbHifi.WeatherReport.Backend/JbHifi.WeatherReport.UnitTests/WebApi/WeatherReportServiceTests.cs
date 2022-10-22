using System.Net;
using JbHifi.WeatherReport.Common;
using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;
using JbHifi.WeatherReport.WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace JbHifi.WeatherReport.UnitTests.WebApi;

[TestClass]
public class WeatherReportServiceTests  
{
    private Mock<IOpenWeatherServiceApiKeyRepository> _openWeatherServiceApiKeyRepository;
    private Mock<IWeatherReportApiKeyRepository> _weatherReportApiKeyRepository;
    private Mock<IAuditRepository> _auditRepository;
    private Mock<IErrorService> _errorService;
    private Mock<ITransformService> _transformService;
    private IHttpClientFactory? _httpClientFactory;
    private string _generatedKey;

    [TestInitialize]
    public void Setup()
    {
        _openWeatherServiceApiKeyRepository = new Mock<IOpenWeatherServiceApiKeyRepository>();
        _weatherReportApiKeyRepository = new Mock<IWeatherReportApiKeyRepository>();
        _auditRepository = new Mock<IAuditRepository>();
        _errorService = new Mock<IErrorService>();
        _transformService = new Mock<ITransformService>(); 
        
        var serviceProvider = new ServiceCollection()  
            .AddHttpClient()
            .BuildServiceProvider();  
        _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        Assert.IsNotNull(_httpClientFactory);
        
        var list = new List<Openweatherserviceapikey>()
        {
            new()
            {
                Id = 1,
                Apikey = "8b7535b42fe1c551f18028f64e8688f7",
                Createdby = "some user",
                Createddate = DateTime.UtcNow,
                Updatedby = "some user",
                Updateddate = DateTime.UtcNow
            },
            new()
            {
                Id = 2,
                Apikey = "9f933451cebf1fa39de168a29a4d9a79",
                Createdby = "some user",
                Createddate = DateTime.UtcNow,
                Updatedby = "some user",
                Updateddate = DateTime.UtcNow
            }
        };

        _openWeatherServiceApiKeyRepository.Setup(a => a.GetAll()).ReturnsAsync(list);
        _transformService.Setup(a => a.Process(It.IsAny<string>())).ReturnsAsync("[\"some description\"]");
    }

    [TestMethod]
    public async Task GetWeatherForecast_Success()
    {
        // Arrange 
        var config = new ConfigurationBuilder().AddJsonFile("./appsettings.json").Build();
        var service = new WeatherReportService(config,
            _openWeatherServiceApiKeyRepository.Object,
            _weatherReportApiKeyRepository.Object,
            _auditRepository.Object,
            _errorService.Object,  
            _transformService.Object,
            _httpClientFactory
        );
        
        var name = "some name1";
        var guid = Guid.NewGuid();
        _generatedKey = Helpers.GenerateApiKey(name, guid);
        var weatherReportApiKeys = new List<Weatherreportapikey>()
        {
            new ()
            {
                Id = 1,
                Name = name,
                Uniqueid = guid,
                Ratelimitperhour = 100,
                Createdby = "some user",
                Createddate = DateTime.UtcNow,
                Updatedby = "some user",
                Updateddate = DateTime.UtcNow
            } 
        };

        _weatherReportApiKeyRepository.Setup(a => a.GetAll()).ReturnsAsync(weatherReportApiKeys);
        
        // Act 
        var response = await service.GetWeatherForecast("london", "uk");

        // Assert
        Assert.IsNotNull(response);
        Assert.IsTrue(response.Any());
        Assert.AreEqual("some description", response.FirstOrDefault());
    }

    [TestMethod]
    public async Task ValidateApiKey_Success()
    {
        // Arrange 
        var config = new ConfigurationBuilder().AddJsonFile("./appsettings.json").Build();
        var service = new WeatherReportService(config,
            _openWeatherServiceApiKeyRepository.Object,
            _weatherReportApiKeyRepository.Object,
            _auditRepository.Object,
            _errorService.Object,  
            _transformService.Object,
            _httpClientFactory
        );
        
        var name = "some name1";
        var guid = Guid.NewGuid();
        _generatedKey = Helpers.GenerateApiKey(name, guid);
        var weatherReportApiKeys = new List<Weatherreportapikey>()
        {
            new ()
            {
                Id = 1,
                Name = name,
                Uniqueid = guid,
                Ratelimitperhour = 100,
                Createdby = "some user",
                Createddate = DateTime.UtcNow,
                Updatedby = "some user",
                Updateddate = DateTime.UtcNow
            } 
        };
        _weatherReportApiKeyRepository.Setup(a => a.GetAll()).ReturnsAsync(weatherReportApiKeys);

        // Act 
        await service.ValidateApiKey(_generatedKey);
        
        // Assert 
        Assert.IsTrue(true);
    }
 
    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public async Task ValidateApiKey_InvalidKey_Fail()
    {
        // Arrange 
        var config = new ConfigurationBuilder().AddJsonFile("./appsettings.json").Build();
        var service = new WeatherReportService(config,
            _openWeatherServiceApiKeyRepository.Object,
            _weatherReportApiKeyRepository.Object,
            _auditRepository.Object,
            _errorService.Object,  
            _transformService.Object,
            _httpClientFactory
        );
        
        var name = "some name1";
        var guid = Guid.NewGuid();
        _generatedKey = Helpers.GenerateApiKey(name, guid);
        var weatherReportApiKeys = new List<Weatherreportapikey>()
        {
            new ()
            {
                Id = 1,
                Name = name,
                Uniqueid = guid,
                Ratelimitperhour = 100,
                Createdby = "some user",
                Createddate = DateTime.UtcNow,
                Updatedby = "some user",
                Updateddate = DateTime.UtcNow
            } 
        };
        _weatherReportApiKeyRepository.Setup(a => a.GetAll()).ReturnsAsync(weatherReportApiKeys);
        
        // Act 
        await service.ValidateApiKey("some dodgy key");
        
        // Assert 
        Assert.IsTrue(false); // never get here
    }
    
    [TestMethod]
    [ExpectedException(typeof(BadHttpRequestException))]
    public async Task ValidateApiKey_AboveLimit_Fail()
    {
        // Arrange 
        var config = new ConfigurationBuilder().AddJsonFile("./appsettings.json").Build();
        var service = new WeatherReportService(config,
            _openWeatherServiceApiKeyRepository.Object,
            _weatherReportApiKeyRepository.Object,
            _auditRepository.Object,
            _errorService.Object,  
            _transformService.Object,
            _httpClientFactory
        );
        var name = "some name1";
        var guid = Guid.NewGuid();
        _generatedKey = Helpers.GenerateApiKey(name, guid);
        var weatherReportApiKeys = new List<Weatherreportapikey>()
        {
            new ()
            {
                Id = 1,
                Name = name,
                Uniqueid = guid,
                Ratelimitperhour = 0,
                Createdby = "some user",
                Createddate = DateTime.UtcNow,
                Updatedby = "some user",
                Updateddate = DateTime.UtcNow
            } 
        };
        _weatherReportApiKeyRepository.Setup(a => a.GetAll()).ReturnsAsync(weatherReportApiKeys);
        
        // Act 
        await service.ValidateApiKey(_generatedKey);
        
        // Assert 
        Assert.IsTrue(false); // never get here
    }
}