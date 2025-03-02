using Application.Mappings;

namespace Application.UseCases;

/// <summary>
///     Класс - реализация сценария использования "Сохранение отфильтрованных логов"
/// </summary>
/// <param name="logsRepo"></param>
/// <param name="eventPublisher"></param>
public class SaveFilteredLogsUseCase(
    ILogsRepository logsRepo,
    IEventPublisher eventPublisher)
{
    public async Task Execute(CLogFilterDto filterDto, string targetTableName)
    {
        var domainFilter = LogFilterMapper.ToDomainFilter(filterDto);

        var filteredLogs = logsRepo.GetLogsByFilter(domainFilter);

        logsRepo.SaveFilteredLogs(filteredLogs, targetTableName);

        await eventPublisher.PublishAsync(new FilteredLogsSaved(targetTableName, filteredLogs));
    }
}