namespace Infrastructure.Services;

/// <summary>
///     Реализация сервиса для чтения файлов.
/// </summary>
public class FileReadService : IFileReadService
{
    /// <inheritdoc />
    public bool DoesFileExists(string? filePath)
    {
        return File.Exists(filePath);
    }

    /// <inheritdoc />
    public bool IsValidPath(string filePath)
    {
        return !string.IsNullOrWhiteSpace(filePath);
    }

    /// <inheritdoc />
    public async Task<string> ReadFileContent(string? filePath)
    {
        return await File.ReadAllTextAsync(filePath);
    }
}