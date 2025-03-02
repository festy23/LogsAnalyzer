namespace Application.UseCases;
/// <summary>
/// Класс - реализация сценария использования "Очистка логов"
/// </summary>
/// <param name="logsRepository"></param>
/// <param name="eventPublisher"></param>
public class ClearLogsUseCase(ILogsRepository logsRepository, IEventPublisher eventPublisher)
{
    public async Task Execute()
    {
        logsRepository.ClearLogs();
        await eventPublisher.PublishAsync(new LogsCleared());
    }
}