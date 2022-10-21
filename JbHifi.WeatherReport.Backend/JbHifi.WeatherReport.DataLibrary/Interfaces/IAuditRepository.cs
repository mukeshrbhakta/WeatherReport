using JbHifi.WeatherReport.DataLibrary.Models;

namespace JbHifi.WeatherReport.DataLibrary.Interfaces;

public interface IAuditRepository
{
    Task<IEnumerable<Audit?>> GetForWeatherReportApiKeyIdForPastHour(int id);
    Task<int> Add(Audit audit);
}