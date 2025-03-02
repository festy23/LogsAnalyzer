namespace Application.Interfaces.Infrastructure;

/// <summary>
///     Интерфейс для репозитория логов
/// </summary>
public interface ILogsRepository
{
    /// <summary>
    ///     Получение логов из базы данных
    /// </summary>
    /// <returns></returns>
    List<CLog> GetLogs();

    /// <summary>
    ///     Добавление лога в базу данных
    /// </summary>
    /// <returns></returns>
    void AddLog(CLog log);

    /// <summary>
    ///     Добавляет список логов
    /// </summary>
    /// <param name="logs"></param>
    void AddLogs(List<CLog> logs);

    /// <summary>
    ///     Получение списка логов в зависимости от критериев фильтрации
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    List<CLog> GetLogsByFilter(CLogFilter filter);

    /// <summary>
    ///     Очистка всех логов, что былт в базе данных
    /// </summary>
    void ClearLogs();

    /// <summary>
    ///     Сохранить отфильтрованные логи
    /// </summary>
    /// <param name="logs"></param>
    /// <param name="targetTableName"></param>
    void SaveFilteredLogs(IEnumerable<CLog> logs, string targetTableName);
    /// <summary>
    /// Получение статистики из репозитория
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    LogStatistics GetStatistics(DateTime? from = null, DateTime? to = null);
}