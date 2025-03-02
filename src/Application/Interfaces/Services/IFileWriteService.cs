namespace Application.Interfaces.Services;

/// <summary>
///     Интерфейс для реализации сервиса записи файлов
/// </summary>
public interface IFileWriteService
{
    /// <summary>
    ///     Записывает строковое содержимое в файл по указанному пути
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    /// <param name="content">Данные для записи</param>
    Task WriteFileContentAsync(string path, string content);

    /// <summary>
    ///     Записывает массив байтов в файл по указанному пути
    /// </summary>
    /// <param name="path"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    Task WriteFileBytesAsync(string path, byte[] bytes);

    /// <summary>
    ///     Добавляет содержимое в конец файла по указанному пути. Если файл не существует, он будет создан.
    ///     Работает асинхронно
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    /// <param name="content">Данные для добавления</param>
    Task AppendFileContentAsync(string path, string content);

    /// <summary>
    ///     Метод проверяет валидность пути файла
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    void ValidatePath(string path);

    /// <summary>
    ///     Асинхронно удаляет файл по указанному пути.
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    Task DeleteFileAsync(string path);
}