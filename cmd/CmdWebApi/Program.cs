using Application.DI;
using Application.Interfaces.Web;
using Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;
using Web.DI;
/*
 * Проект 3_2
 * Выполнил: Коновалов Иван
 * 1 вар. B side
 *
 */
namespace CmdWebApi;
/// <summary>
/// Точка входа в REST API
/// </summary>
class Program
{   
    /// <summary>
    /// Точка входа для запуска сервера
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {   
        var services = new ServiceCollection();

        services.AddInfrastructureServices();
        services.AddApplicationServices();
        services.AddWebServices();
        
        var serviceProvider = services.BuildServiceProvider();
    
        var restServer = serviceProvider.GetRequiredService<IRestServer>();
        restServer.StartAsync().Wait();
    }
}