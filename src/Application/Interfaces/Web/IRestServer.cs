namespace Application.Interfaces.Web;
/// <summary>
/// Интерфейс для рест сервера
/// </summary>
public interface IRestServer
{   
    /// <summary>
    /// Асинхронный запуск сервера
    /// </summary>
    /// <returns></returns>
    Task StartAsync();
    /// <summary>
    /// Метод для остановки сервера
    /// </summary>
    void Stop();
}