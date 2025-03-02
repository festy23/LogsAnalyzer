using Application.Mappings;

namespace Application.UseCases;
/// <summary>
/// Класс - реализация сценария использования "Получение статистики логов"
/// </summary>
/// <param name="logsRepository"></param>
public class AddLogUseCase(ILogsRepository logsRepository)
{
    public void Execute(CLogDto logDto)
    {
        var clog = LogMapper.ToDomain(logDto);
        logsRepository.AddLog(clog);
    }
}