using System.Net;

namespace JbHifi.WeatherReport.FrontEnd.WebUI.Models;

public class ResponseViewModel
{
    public List<string>? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }
}