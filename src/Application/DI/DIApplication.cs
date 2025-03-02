using Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI;

/// <summary>
///     Внедрение зависимостей сервисов Application
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<ImportLogsUseCase>();
        services.AddTransient<FilterLogsUseCase>();
        services.AddTransient<ClearLogsUseCase>();
        services.AddTransient<ExportLogsUseCase>();
        services.AddTransient<SaveFilteredLogsUseCase>();
        services.AddTransient<MakeLogPlotsUseCase>();
        services.AddTransient<AddLogUseCase>();
        services.AddTransient<GetLogsUseCase>();
        services.AddTransient<GetLogsStatisticsUseCase>();
        return services;
    }
}