using Application.Interfaces.Events;
using Application.Interfaces.Parser;
using Infrastructure.Parsers;
using Infrastructure.Repository;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;
/// <summary>
/// Внедрение зависимостей
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ILogsRepository, SqliteLogsRepository>();
        services.AddSingleton<IFileReadService, FileReadService>();
        services.AddSingleton<IFileWriteService, CFileWriteService>();
        services.AddSingleton<ILogParser, LogParser>();
        services.AddSingleton<IEventPublisher, EventPublisher>();
        services.AddSingleton<IPlotService, ScottPlotService>();
        return services;
    }
}