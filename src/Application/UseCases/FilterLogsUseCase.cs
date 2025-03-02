using Application.Mappings;

namespace Application.UseCases;
/// <summary>
/// Класс - реализация сценария использования "Фильтрование логов"
/// </summary>
/// <param name="logsRepo"></param>
/// <param name="eventPublisher"></param>
public class FilterLogsUseCase(
    ILogsRepository logsRepo,
    IEventPublisher eventPublisher)
{
    public async Task<List<CLog>> Execute(CLogFilterDto filterDto)
    {
        var domainFilter = LogFilterMapper.ToDomainFilter(filterDto);

        var filteredLogs = logsRepo.GetLogsByFilter(domainFilter);

        await eventPublisher.PublishAsync(new LogsFiltered(filterDto, filteredLogs));

        return filteredLogs;
    }
}