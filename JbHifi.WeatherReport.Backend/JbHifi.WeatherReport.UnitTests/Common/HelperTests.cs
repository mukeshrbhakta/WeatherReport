using System.Diagnostics;
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
        // Act 
        var clearText = Helpers.GetDatabaseConnectionString(_configuration);
        
        // Assert
        Assert.AreEqual("some text", clearText);
    }

    [TestMethod]
    public void GenerateApiKey_Success()
    {
        // Arrange 
        var name = "name";
        var guid = Guid.NewGuid();
        
        // Act 
        var apiKey = Helpers.GenerateApiKey(name, guid);
        
        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(apiKey)); 
    }
    
    [Conditional("DEBUG")]
    [TestMethod, Ignore]
    public void GenerateAllApiKeys_Success()
    { 
        // Act 
        var apiKey1 = Helpers.GenerateApiKey("Key #1", new Guid("your guid"));
        var apiKey2 = Helpers.GenerateApiKey("Key #2", new Guid("your guid"));
        var apiKey3 = Helpers.GenerateApiKey("Key #3", new Guid("your guid"));
        var apiKey4 = Helpers.GenerateApiKey("Key #4", new Guid("your guid"));
        var apiKey5 = Helpers.GenerateApiKey("Key #5", new Guid("your guid"));
        
        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(apiKey1)); 
        Assert.IsFalse(string.IsNullOrWhiteSpace(apiKey2)); 
        Assert.IsFalse(string.IsNullOrWhiteSpace(apiKey3)); 
        Assert.IsFalse(string.IsNullOrWhiteSpace(apiKey4)); 
        Assert.IsFalse(string.IsNullOrWhiteSpace(apiKey5)); 
    }
}