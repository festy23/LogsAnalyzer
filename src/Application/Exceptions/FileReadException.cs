namespace Application.Exceptions;

/// <summary>
///     Исключение вызываемое при ошибке чтения файла
/// </summary>
/// <param name="message"></param>
public class FileReadException(string message) : Exception(message);