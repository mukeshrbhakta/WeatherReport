using JbHifi.WeatherReport.Common;
using Microsoft.Extensions.Configuration;

namespace JbHifi.WeatherReport.UnitTests.Common;

[TestClass]
public class HelperTests
{ 
    private IConfiguration? _configuration { get; set; }
    
    [TestInitialize]
    public void Setup()
    {
        _configuration = new ConfigurationBuilder().AddJsonFile("./appsettings.json").Build();
    }

    [TestMethod]
    public void GetDatabaseConnectionString_Success()
    {
        // Arrange 
        
        // Act 
        var clearText = Helpers.GetDatabaseConnectionString(_configuration);
        
        // Assert
        Assert.AreEqual(clearText, "some text");
    }

    [TestMethod]
    public void GenerateApiKey_Success()
    {
        // Arrange 
        var name = "Key #1";
        var guid = new Guid("363ba294-b4b3-4e7b-ae57-c8a2289bb5c7"); // Guid.NewGuid();
        
        // Act 
        var apiKey = Helpers.GenerateApiKey(_configuration, name, guid);
        
        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(apiKey)); 
    }
}