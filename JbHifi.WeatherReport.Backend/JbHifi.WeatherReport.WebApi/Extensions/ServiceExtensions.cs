using JbHifi.WeatherReport.DataLibrary.Implementations;
using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.WebApi.Attributes;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using JbHifi.WeatherReport.WebApi.Services;

namespace JbHifi.WeatherReport.WebApi.Extensions;

/// <summary>
/// The service extension
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Add services
    /// </summary>
    /// <param name="builder"></param>
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.AddLogging();
        
        var services = builder.Services;
        services.AddRouting(options => options.LowercaseUrls = true);
        
        services.AddScoped<IWeatherReportDbFactory, WeatherReportDbFactory>();
        services.AddScoped<IWeatherReportApiKeyRepository, WeatherReportApiKeyRepository>();
        services.AddScoped<IOpenWeatherServiceApiKeyRepository, OpenWeatherServiceApiKeyRepository>();
        services.AddScoped<IWeatherReportService, WeatherReportService>();
        services.AddScoped<IErrorService, ErrorService>();
        services.AddScoped<ITransformService, TransformService>();
        services.AddScoped<AuthorizeAttribute>();
        services.AddHttpClient();
    }

    /// <summary>
    /// Add logger
    /// </summary>
    /// <param name="builder"></param>
    private static void AddLogging(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration.AddJsonFile("./appsettings.json").Build();
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File(config["LogFileName"],
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();
        builder.Services.AddLogging(a => a.AddSerilog());
    }

}