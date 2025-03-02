namespace Application.Exceptions;

/// <summary>
///     Исключение вызываемое при ошибки записи файла
/// </summary>
/// <param name="message"></param>
public class FileWriteException(string message) : Exception(message);