namespace Application.Interfaces.Services;

public interface IPlotService
{
    /// <summary>
    ///     Генерирует график количества логов по времени и сохраняет его в файл.
    /// </summary>
    /// <param name="logs">Список логов</param>
    /// <param name="outputFilePath">Путь для сохранения графика</param>
    /// <returns>Путь к сохраненному файлу</returns>
    Task<string> GenerateLogsCountPlotAsync(IEnumerable<CLog> logs, string outputFilePath);

    /// <summary>
    ///     Генерирует график количества ошибок по времени и сохраняет его в файл.
    /// </summary>
    /// <param name="logs">Список логов</param>
    /// <param name="outputFilePath">Путь для сохранения графика</param>
    /// <returns>Путь к сохраненному файлу</returns>
    Task<string> GenerateErrorCountPlotAsync(IEnumerable<CLog> logs, string outputFilePath);
}