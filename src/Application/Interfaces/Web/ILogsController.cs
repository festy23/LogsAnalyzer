using System.Net;

namespace Application.Interfaces.Web;
/// <summary>
/// Интерфейс для контроллера логов 
/// </summary>
public interface ILogsController
{   
    /// <summary>
    /// Асинхронная обработка post запроса
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task HandlePost(HttpListenerContext context);
    /// <summary>
    /// Асинхронная обработка get запроса
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task HandleGet(HttpListenerContext context);
    /// <summary>
    /// Асинхронная обработка get запроса по статистике
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task HandleGetStatistics(HttpListenerContext context);
}
