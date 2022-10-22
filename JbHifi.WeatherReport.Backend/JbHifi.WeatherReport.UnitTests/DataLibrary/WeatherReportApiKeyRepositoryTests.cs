using JbHifi.WeatherReport.DataLibrary.Implementations;
using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace JbHifi.WeatherReport.UnitTests.DataLibrary;

[TestClass]
public class WeatherReportApiKeyRepositoryTests
{
     private Mock<IWeatherReportDbFactory>? _weatherReportDbFactory;
    private Mock<WeatherReportDbContext>? _weatherReportDbContext;
    private Mock<DbSet<Weatherreportapikey>>? _weatherReportApiKeyDbSet; 


    [TestInitialize]
    public void Setup()
    {
        _weatherReportDbFactory = new Mock<IWeatherReportDbFactory>();
        _weatherReportDbContext = new Mock<WeatherReportDbContext>();
        _weatherReportDbFactory.Setup(a => a.GetDbContext()).Returns(_weatherReportDbContext.Object);

        var list = new List<Weatherreportapikey>()
        {
            new ()
            {
                Id = 1,
                Name = "some name1",
                Uniqueid = Guid.NewGuid(),
                Ratelimitperhour = 100,
                Createdby = "some user",
                Createddate = DateTime.UtcNow,
                Updatedby = "some user",
                Updateddate = DateTime.UtcNow
            },
            new ()
            {
                Id = 2,
                Name = "some name2",
                Uniqueid = Guid.NewGuid(),
                Ratelimitperhour = 1,
                Createdby = "some user",
                Createddate = DateTime.UtcNow,
                Updatedby = "some user",
                Updateddate = DateTime.UtcNow
            }
        }.AsQueryable();

        _weatherReportApiKeyDbSet = new Mock<DbSet<Weatherreportapikey>>();
        _weatherReportApiKeyDbSet.As<IQueryable<Weatherreportapikey>>().Setup(m => m.Provider).Returns(list.Provider);
        _weatherReportApiKeyDbSet.As<IQueryable<Weatherreportapikey>>().Setup(m => m.Expression).Returns(list.Expression);
        _weatherReportApiKeyDbSet.As<IQueryable<Weatherreportapikey>>().Setup(m => m.ElementType).Returns(list.ElementType);
        _weatherReportApiKeyDbSet.As<IQueryable<Weatherreportapikey>>().Setup(m => m.GetEnumerator()).Returns(() => list.GetEnumerator());
        
        _weatherReportDbContext.Setup(a => a.Weatherreportapikeys).Returns(_weatherReportApiKeyDbSet.Object!);
    }
    
    [TestMethod]
    public async Task GetAll_Success()
    {
        // Arrange 
        var repository = new WeatherReportApiKeyRepository(_weatherReportDbFactory!.Object);

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
        var repository = new WeatherReportApiKeyRepository(_weatherReportDbFactory!.Object);

        // Act 
        var response = await repository.Get(2);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(2, response.Id);
        Assert.AreEqual(1, response.Ratelimitperhour);
    }
    
    [TestMethod]
    public async Task Get_NonExisting_Success()
    {
        // Arrange 
        var repository = new WeatherReportApiKeyRepository(_weatherReportDbFactory!.Object);

        // Act 
        var response = await repository.Get(100);

        // Assert
        Assert.IsNull(response); 
    }
}