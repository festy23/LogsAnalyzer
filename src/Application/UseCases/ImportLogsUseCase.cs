namespace Application.UseCases;

/// <summary>
///     Класс - реализация сценария использования "Импорт логов"
/// </summary>
public class ImportLogsUseCase(
    IFileReadService fileReader,
    ILogParser logParser,
    ILogsRepository logsRepo,
    IEventPublisher eventPublisher)
{
    public async Task Execute(string filePath)
    {
        if (!fileReader.IsValidPath(filePath)) throw new FileWriteException("Некорректный путь к файлу");

        if (!fileReader.DoesFileExists(filePath)) throw new FileWriteException("Файл не существует");

        var rawContent = await fileReader.ReadFileContent(filePath);
        // тут мы получаем в сыром виде эти строки, разбиваем их со Split, чтобы можно было юзать ParseToList
        var rawLogs = rawContent
            .Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);

        var logs = logParser.ParseToList(rawLogs)
                   ?? throw new InvalidDataException("Неверный формат файла");

        logsRepo.AddLogs(logs);

        await eventPublisher.PublishAsync(new LogsImported(logs));
    }
}