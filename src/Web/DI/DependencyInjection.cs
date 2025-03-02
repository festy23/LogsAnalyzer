using Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using Web.Controllers;
using Web.Server;

namespace Web.DI;
/// <summary>
/// Внедряем зависимости для Web слоя
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<ILogsController, LogsController>(provider =>
            new LogsController(
                provider.GetRequiredService<AddLogUseCase>(),
                provider.GetRequiredService<GetLogsUseCase>(),
                provider.GetRequiredService<GetLogsStatisticsUseCase>()
            ));

        services.AddSingleton<IRestServer, RestServer>(provider =>
            new RestServer(
                "http://localhost:5000/", 
                provider.GetRequiredService<ILogsController>()
            ));

        return services;
    }
}
