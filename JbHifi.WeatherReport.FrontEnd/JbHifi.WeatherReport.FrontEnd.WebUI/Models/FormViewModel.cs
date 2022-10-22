namespace JbHifi.WeatherReport.FrontEnd.WebUI.Models;

public class FormViewModel
{
    public FormViewModel()  
    {
        
    }
    
    public FormViewModel(string? city, string? country, string? response = null)
    {
        City = city;
        Country = country;
        Response = response;
    }
    
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Response { get; set; }
}