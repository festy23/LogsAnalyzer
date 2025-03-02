namespace Application.Interfaces.Services;

/// <summary>
///     Интерфейс для реализации сервиса чтения файлов
/// </summary>
public interface IFileReadService
{
    /// <summary>
    ///     Проверяет корректность пути файла
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    /// <returns></returns>
    bool IsValidPath(string path);

    /// <summary>
    ///     Проверят, существует ли файл по данному пути
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    /// <returns></returns>
    bool DoesFileExists(string? path);

    /// <summary>
    ///     Читает содержимое файла, по данному пути. Работает асинхронно
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    Task<string> ReadFileContent(string? path);
}