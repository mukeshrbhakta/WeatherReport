using System.Text.Json;
using JbHifi.WeatherReport.Common;

namespace JbHifi.WeatherReport.UnitTests.Common;

[TestClass]
public class ExtensionMethodsTests
{
    [TestMethod]
    public void ToJsonArray_Success()
    {
        // Arrange 
        var list = new List<string>()
        {
            "{ \"name\" : \"first item\"}",
            "{ \"name\" : \"second item\"}",
        };
        
        // Act
        var response = list.ToJsonArray(); 
        
        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(response));
        Assert.IsTrue(IsValidJsonArray(response)); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(JsonException))]
    public void ToJsonArray_InvalidData_Fail()
    {
        // Arrange 
        var list = new List<string>()
        {
            "item 1",
            "item 2"
        };
        
        // Act
        var response = list.ToJsonArray();
        
        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(response));
        Assert.IsFalse(IsValidJsonArray(response));
    }
    
    [TestMethod] 
    public void ToJsonArray_EmptyContainer_Fail()
    {
        // Arrange 
        var list = new List<string>();
        
        // Act
        var response = list.ToJsonArray();
        
        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(response));
        Assert.IsTrue(IsValidJsonArray(response));
    }
 
    private bool IsValidJsonArray(string response)
    {
        var json = JsonSerializer.Deserialize<List<dynamic>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        return json != null;
    }
}