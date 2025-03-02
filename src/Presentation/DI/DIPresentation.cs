using Microsoft.Extensions.DependencyInjection;

namespace Presentation.DI;
/// <summary>
/// Внедряем зависимости для Presentation слоя
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddScoped<IMenu, CMenu>();
        services.AddScoped<ILogMenuVisualizer, CLogVisualizer>();
        services.AddScoped<CMenuDecorator>();
        return services;
    }
}