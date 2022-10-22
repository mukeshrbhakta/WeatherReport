using JbHifi.WeatherReport.DataLibrary.Implementations;
using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace JbHifi.WeatherReport.UnitTests.DataLibrary;

[TestClass]
public class OpenWeatherServiceApiKeyRepositoryTests
{
    private Mock<IWeatherReportDbFactory>? _weatherReportDbFactory;
    private Mock<WeatherReportDbContext>? _weatherReportDbContext;
    private Mock<DbSet<Openweatherserviceapikey>>? _openWeatherServiceApiKeyDbSet; 


    [TestInitialize]
    public void Setup()
    {
        _weatherReportDbFactory = new Mock<IWeatherReportDbFactory>();
        _weatherReportDbContext = new Mock<WeatherReportDbContext>();
        _weatherReportDbFactory.Setup(a => a.GetDbContext()).Returns(_weatherReportDbContext.Object);

        var list = new List<Openweatherserviceapikey>()
        {
            new ()
            {
                Id = 1,
                Apikey = "some key1",
                Createdby = "some user",
                Createddate = DateTime.UtcNow,
                Updatedby = "some user",
                Updateddate = DateTime.UtcNow
            },
            new ()
            {
                Id = 2,
                Apikey = "some key2",
                Createdby = "some user",
                Createddate = DateTime.UtcNow,
                Updatedby = "some user",
                Updateddate = DateTime.UtcNow
            } 
        }.AsQueryable();

        _openWeatherServiceApiKeyDbSet = new Mock<DbSet<Openweatherserviceapikey>>();
        _openWeatherServiceApiKeyDbSet.As<IQueryable<Openweatherserviceapikey>>().Setup(m => m.Provider).Returns(list.Provider);
        _openWeatherServiceApiKeyDbSet.As<IQueryable<Openweatherserviceapikey>>().Setup(m => m.Expression).Returns(list.Expression);
        _openWeatherServiceApiKeyDbSet.As<IQueryable<Openweatherserviceapikey>>().Setup(m => m.ElementType).Returns(list.ElementType);
        _openWeatherServiceApiKeyDbSet.As<IQueryable<Openweatherserviceapikey>>().Setup(m => m.GetEnumerator()).Returns(() => list.GetEnumerator());
        
        _weatherReportDbContext.Setup(a => a.Openweatherserviceapikeys).Returns(_openWeatherServiceApiKeyDbSet.Object!);
    }
    
    [TestMethod]
    public async Task GetAll_Success()
    {
        // Arrange 
        var repository = new OpenWeatherServiceApiKeyRepository(_weatherReportDbFactory!.Object);

        // Act 
        var response = await repository.GetAll();

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(2, response.Count());
    }
    
    [TestMethod]
    public async Task Get_Success()
    {
        // Arrange 
        var repository = new OpenWeatherServiceApiKeyRepository(_weatherReportDbFactory!.Object);

        // Act 
        var response = await repository.Get(2);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(2, response.Id);
        Assert.AreEqual("some key2", response.Apikey);
    }
    
    [TestMethod]
    public async Task Get_NonExisting_Success()
    {
        // Arrange 
        var repository = new OpenWeatherServiceApiKeyRepository(_weatherReportDbFactory!.Object);

        // Act 
        var response = await repository.Get(100);

        // Assert
        Assert.IsNull(response); 
    }
}