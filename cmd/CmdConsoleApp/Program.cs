using Application.DI;
using Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;
using Presentation.DI;
using Presentation.Interfaces;

namespace Cmd;
/*
 * Проект 3_2
 * Выполнил: Коновалов Иван
 * 1 вар. B side
 * 
 */
/// <summary>
///     Класс содержит точку входа в консольное приложение - Main
/// </summary>
internal class Program
{
    /// <summary>
    ///     Точка входа в консольное приложение
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddInfrastructureServices();
        services.AddApplicationServices();
        services.AddPresentationServices();

        var serviceProvider = services.BuildServiceProvider();

        var menu = serviceProvider.GetRequiredService<IMenu>();
        menu.Run();
    }
}
