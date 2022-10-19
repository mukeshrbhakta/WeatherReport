using JbHifi.WeatherReport.DataLibrary.Models;

namespace JbHifi.WeatherReport.DataLibrary.Interfaces;

public interface IWeatherReportDbFactory
{
    WeatherReportDbContext GetDbContext();
}