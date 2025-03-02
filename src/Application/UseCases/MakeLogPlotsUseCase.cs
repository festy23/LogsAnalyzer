namespace Application.UseCases;
/// <summary>
/// Класс - реализация сценария использования "Создание графика логов"
/// </summary>
/// <param name="logsRepository"></param>
/// <param name="plotService"></param>
public class MakeLogPlotsUseCase(ILogsRepository logsRepository, IPlotService plotService)
{
    public async Task<(string LogCountPlotPath, string ErrorCountPlotPath)> ExecuteAsync(string logCountOutputPath,
        string errorCountOutputPath)
    {
        var logs = logsRepository.GetLogs();
        var logCountPlotPath = await plotService.GenerateLogsCountPlotAsync(logs, logCountOutputPath);
        var errorCountPlotPath = await plotService.GenerateErrorCountPlotAsync(logs, errorCountOutputPath);
        return (logCountPlotPath, errorCountPlotPath);
    }
}