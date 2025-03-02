namespace Infrastructure.Services;

public class CFileWriteService : IFileWriteService
{
    /// <inheritdoc />
    public async Task WriteFileContentAsync(string path, string content)
    {
        try
        {
            ValidatePath(path);
            await File.WriteAllTextAsync(path, content);
        }
        catch (IOException ioEx)
        {
            throw new IOException("Ошибка при записи файла.", ioEx);
        }
        catch (Exception ex)
        {
            throw new Exception("Неизвестная ошибка при записи файла.", ex);
        }
    }

    /// <inheritdoc />
    public async Task AppendFileContentAsync(string path, string content)
    {
        ValidatePath(path);
        try
        {
            await File.AppendAllTextAsync(path, content);
        }
        catch (IOException ioEx)
        {
            throw new IOException("Ошибка при добавлении содержимого в файл.", ioEx);
        }
        catch (Exception ex)
        {
            throw new Exception("Неизвестная ошибка при добавлении содержимого в файл.", ex);
        }
    }

    /// <inheritdoc />
    public async Task DeleteFileAsync(string path)
    {
        ValidatePath(path);
        try
        {
            if (File.Exists(path))
                // File.Delete является синхронным, поэтому оборачиваем его в Task.Run для асинхронного выполнения
                await Task.Run(() => File.Delete(path));
            else
                throw new FileNotFoundException("Файл не найден.", path);
        }
        catch (IOException ioEx)
        {
            throw new IOException("Ошибка при удалении файла.", ioEx);
        }
        catch (Exception ex)
        {
            throw new Exception("Неизвестная ошибка при удалении файла.", ex);
        }
    }

    /// <inheritdoc />
    public void ValidatePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Путь не может быть пустым или состоять только из пробелов.", nameof(path));

        try
        {
            // Получаем полный путь для проверки корректности синтаксиса
            var fullPath = Path.GetFullPath(path);

            // Дополнительная проверка: файл должен иметь расширение
            if (!Path.HasExtension(fullPath))
                throw new ArgumentException("Путь должен содержать расширение файла.", nameof(path));

            // Проверка на недопустимые символы в имени файла
            var fileName = Path.GetFileName(fullPath);
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException("Имя файла содержит недопустимые символы.", nameof(path));
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Некорректный формат пути.", nameof(path), ex);
        }
    }

    public async Task WriteFileBytesAsync(string path, byte[] bytes)
    {
        ValidatePath(path);
        try
        {
            await File.WriteAllBytesAsync(path, bytes);
        }
        catch (IOException ioEx)
        {
            throw new IOException("Ошибка при записи бинарных данных в файл.", ioEx);
        }
        catch (Exception ex)
        {
            throw new Exception("Неизвестная ошибка при записи бинарных данных в файл.", ex);
        }
    }
}