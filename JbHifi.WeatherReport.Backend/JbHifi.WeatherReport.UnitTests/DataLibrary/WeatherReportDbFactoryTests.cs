using JbHifi.WeatherReport.DataLibrary.Implementations;
using Microsoft.Extensions.Configuration;

namespace JbHifi.WeatherReport.UnitTests.DataLibrary;

[TestClass] 
public class WeatherReportDbFactoryTests
{
    [TestMethod]
    public void Constructor_Success()
    {
        // Arrange 
        var config = new ConfigurationBuilder().AddJsonFile("./appsettings.json").Build();
        var factory = new WeatherReportDbFactory(config);
        
        // Assert
        Assert.IsNotNull(factory);
    }
    
    [TestMethod]
    public void GetDbContext_Success()
    {
        // Arrange 
        var config = new ConfigurationBuilder().AddJsonFile("./appsettings.json").Build();
        var factory = new WeatherReportDbFactory(config);
        
        // Act 
        var dbContext = factory.GetDbContext();
        
        // Assert
        Assert.IsNotNull(dbContext);
    }
}