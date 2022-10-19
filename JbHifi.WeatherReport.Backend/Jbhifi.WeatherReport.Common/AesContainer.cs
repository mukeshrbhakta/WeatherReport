namespace JbHifi.WeatherReport.Common;

/// <summary>
/// The AES container
/// </summary>
public class AesContainer
{
    /// <summary>
    /// The key 
    /// </summary>
    public string? Key { get; set; }
    
    /// <summary>
    /// The IV 
    /// </summary>
    public string? Iv { get; set; }
}