namespace JbHifi.WeatherReport.WebApi.Services;

public interface IErrorService
{
    Guid LogException(Exception exception);
}