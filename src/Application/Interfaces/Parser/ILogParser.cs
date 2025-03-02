namespace Application.Interfaces.Parser;

/// <summary>
///     Интерфейс для парсера логов
/// </summary>
public interface ILogParser
{
    /// <summary>
    ///     Метод для парсинга строки в лог
    /// </summary>
    /// <param name="rawLog"></param>
    /// <returns></returns>
    CLog Parse(string rawLog);

    /// <summary>
    ///     Метод для парсинга коллекции строк в список логов
    /// </summary>
    /// <param name="rawLogs"></param>
    /// <returns></returns>
    List<CLog> ParseToList(IEnumerable<string> rawLogs);
}