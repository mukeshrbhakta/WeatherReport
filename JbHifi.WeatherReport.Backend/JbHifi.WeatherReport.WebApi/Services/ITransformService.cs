namespace JbHifi.WeatherReport.WebApi.Services;

public interface ITransformService
{
    Task<string> Process(string request);
}