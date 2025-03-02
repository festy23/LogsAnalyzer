namespace Application.UseCases;
/// <summary>
/// Класс - реализация сценария использования "Получение статистики логов"
/// </summary>
/// <param name="logsRepository"></param>
/// <param name="eventPublisher"></param>
public class GetLogsStatisticsUseCase(ILogsRepository logsRepository, IEventPublisher eventPublisher)
{
    public async Task<LogStatistics> Execute(CLogsStatisticsDto dto)
    {
        var statistics = logsRepository.GetStatistics(dto.DtoFrom, dto.DtoTo);
            
        await eventPublisher.PublishAsync(new CLogsStatisticsDto(dto.DtoTo, dto.DtoFrom));
            
        return statistics;
    }
}