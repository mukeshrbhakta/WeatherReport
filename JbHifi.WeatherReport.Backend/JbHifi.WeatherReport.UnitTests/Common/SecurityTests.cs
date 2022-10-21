using JbHifi.WeatherReport.Common;
using Microsoft.Extensions.Configuration;

namespace JbHifi.WeatherReport.UnitTests.Common;

[TestClass]
public class SecurityTests
{
    private string? _key { get; set; }
    [TestInitialize]
    public void Setup()
    {
        var config = new ConfigurationBuilder().AddJsonFile("./appsettings.json").Build();
        _key = config["Key"];
    }
    
    [TestMethod]
    public void CreateAes()
    {
        // Arrange
        var aes = Security.CreateAes();
        
        // Act 

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(aes.Key));
        Assert.IsFalse(string.IsNullOrWhiteSpace(aes.Iv));
    }

    [TestMethod]
    public void EncDec_Success()
    {
        // Arrange
        var clearText = "some text"; 
        
        // Act 
        var encText = Security.Encrypt(_key, clearText);
        var decText = Security.Decrypt(_key, encText);
        
        // Assert 
        Assert.AreEqual(clearText, decText);

    }
}