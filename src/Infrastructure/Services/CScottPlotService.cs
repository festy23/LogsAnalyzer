using ScottPlot;

namespace Infrastructure.Services;

/// <summary>
///     Класс реализующий сервис по построению графиков, используя ScottPlot
/// </summary>
/// <param name="fileWriteService"></param>
public class ScottPlotService(IFileWriteService fileWriteService) : IPlotService
{
    public async Task<string> GenerateLogsCountPlotAsync(IEnumerable<CLog> logs, string outputFilePath)
    {
        ArgumentNullException.ThrowIfNull(logs);
        fileWriteService.ValidatePath(outputFilePath);

        var (xs, ys) = GroupLogsByHour(logs);
        var plt = CreateScatterPlot(xs, ys, "Количество логов по времени", "Время", "Количество логов");

        var imageBytes = plt.GetImageBytes(600, 400, ImageFormat.Png);
        await fileWriteService.WriteFileBytesAsync(outputFilePath, imageBytes);
        return outputFilePath;
    }

    public async Task<string> GenerateErrorCountPlotAsync(IEnumerable<CLog> logs, string outputFilePath)
    {
        ArgumentNullException.ThrowIfNull(logs);
        fileWriteService.ValidatePath(outputFilePath);

        var errorLogs = logs.Where(log =>
            string.Equals(log.LevelOfImportance, "ERROR", StringComparison.OrdinalIgnoreCase));
        var (xs, ys) = GroupLogsByHour(errorLogs);
        var plt = CreateScatterPlot(xs, ys, "Количество ошибок по времени", "Время", "Количество ошибок");
        var imageBytes = plt.GetImageBytes(600, 400, ImageFormat.Png);

        await fileWriteService.WriteFileBytesAsync(outputFilePath, imageBytes);
        return outputFilePath;
    }

    /// <summary>
    ///     Группировка логов по часам
    /// </summary>
    /// <param name="logs"></param>
    /// <returns></returns>
    private (double[] xs, double[] ys) GroupLogsByHour(IEnumerable<CLog> logs)
    {
        var grouped = logs.GroupBy(log =>
                new DateTime(log.DateAndTime.Year, log.DateAndTime.Month, log.DateAndTime.Day, log.DateAndTime.Hour, 0,
                    0))
            .OrderBy(g => g.Key)
            .Select(g => new { Time = g.Key, Count = g.Count() })
            .ToList();

        var xs = grouped.Select(item => item.Time.ToOADate()).ToArray();
        var ys = grouped.Select(item => (double)item.Count).ToArray();
        return (xs, ys);
    }

    /// <summary>
    ///     Создание графика
    /// </summary>
    /// <param name="xs"></param>
    /// <param name="ys"></param>
    /// <param name="title"></param>
    /// <param name="xLabel"></param>
    /// <param name="yLabel"></param>
    /// <returns></returns>
    private Plot CreateScatterPlot(double[] xs, double[] ys, string title, string xLabel, string yLabel)
    {
        var plt = new Plot();
        plt.Add.Scatter(xs, ys, new Color(255, 0, 0));
        plt.Axes.DateTimeTicksBottom();
        plt.Title(title);
        plt.XLabel(xLabel);
        plt.YLabel(yLabel);
        return plt;
    }
}