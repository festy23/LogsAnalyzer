using Application.Mappings;

namespace Application.UseCases;
/// <summary>
/// Класс - реализация сценария использования "Получение логов"
/// </summary>
/// <param name="logsRepository"></param>
public class GetLogsUseCase(ILogsRepository logsRepository)
{
    public IEnumerable<CLogDto> Execute(DateTime? from = null, DateTime? to = null)
    {
        List<CLog> logs;
        if (!from.HasValue && !to.HasValue)
        {
            logs = logsRepository.GetLogs();
            
        }
        else
        {
            var filter = new CLogFilter(null, from, to, null, false);
            logs = logsRepository.GetLogsByFilter(filter);
        }

        return logs.Select(log => LogMapper.ToDto(log)).ToList();
    }
}