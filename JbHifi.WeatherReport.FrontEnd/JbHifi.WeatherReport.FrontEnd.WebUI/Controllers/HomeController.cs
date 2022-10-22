using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using JbHifi.WeatherReport.FrontEnd.WebUI.Models;

namespace JbHifi.WeatherReport.FrontEnd.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController( 
        IConfiguration configuration, 
        IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index(string city, string country, string response)
    {
        var model = new FormViewModel()
        {
            City = string.IsNullOrWhiteSpace(city) ? "Melbourne" : city,
            Country = string.IsNullOrWhiteSpace(country) ?  "Australia" : country,
            Response = response
        };

        if (string.IsNullOrWhiteSpace(city) && string.IsNullOrWhiteSpace(country))
        {
            // Do not call if data is already provided
            var responseViewModel = await MakeWebRequest(model.City, model.Country);
            model.Response = GetResponseString(responseViewModel);
        }

        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Index(FormViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.City) && string.IsNullOrWhiteSpace(model.Country))
        {
            model.City = "Melbourne";
            model.Country = "Australia";
        }
        
        var response = await MakeWebRequest(model.City, model.Country);
        var responseString = GetResponseString(response);
        
        return RedirectToAction("Index", new FormViewModel(model.City, model.Country, responseString));
    }

    public IActionResult Privacy()
    {
        return View();
    }
     
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
 
    private async Task<ResponseViewModel> MakeWebRequest(string? city, string? country)
    {
        var result = new ResponseViewModel();

        try
        {
            // Get the values
            var endPoint = _configuration["ServiceEndpoint"];
            var apiKey = _configuration["Key"]; 
            var uri = new Uri($"{endPoint}getweatherforecast?city={city}&country={country}");
            
            // Make the request 
            using var httpClient = _httpClientFactory.CreateClient("MyClient"); 
            httpClient.DefaultRequestHeaders.Add("API-KEY", apiKey);
            var response = await httpClient.GetAsync(uri);
             
            // Collate the response
            result.HttpStatusCode = response.StatusCode;
            var data = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            { 
                result.Data = FromJson(data);
            }
            else
            {
                result.ErrorMessage = data;
            }
        }
        catch (Exception exception)
        {
            // Report exception to UI
            result.HttpStatusCode = HttpStatusCode.InternalServerError;
            result.ErrorMessage = exception.Message;
        }

        return result;
    }

    private string GetResponseString(ResponseViewModel model)
    {
        var result = string.Empty;
        if (model.HttpStatusCode == HttpStatusCode.OK)
        { 
            if (model.Data != null)
            {
                var bldr = new StringBuilder();
                foreach (var item in model.Data)
                {
                    bldr.Append($"{item},");
                }

                result = bldr.ToString();
            }
        }
        else
        {
            var errMsg = model.HttpStatusCode == HttpStatusCode.NotFound ? "not found" : model.ErrorMessage;
            result = $"Error : {errMsg}";
        }

        return result;
    }

    private List<string>? FromJson(string payload)
    {
        return JsonSerializer.Deserialize<List<string>>(payload, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        }); 
    }

    
}