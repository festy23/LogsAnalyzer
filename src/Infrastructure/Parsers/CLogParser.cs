using System.Text.RegularExpressions;
using Application.Interfaces.Parser;

namespace Infrastructure.Parsers;
/// <summary>
/// Класс для парсинга логов
/// </summary>
public partial class LogParser : ILogParser
{
    private static readonly Regex LogRegex = MyRegex();

    public CLog Parse(string rawLog)
    {
        if (string.IsNullOrWhiteSpace(rawLog))
            throw new ArgumentException("Исходная строка лога пуста", nameof(rawLog));

        var match = LogRegex.Match(rawLog);
        if (!match.Success)
            throw new FormatException("Неверный формат строки лога");

        var dateStr = match.Groups["date"].Value;
        var level = match.Groups["level"].Value;
        var message = match.Groups["message"].Value;

        if (!DateTime.TryParseExact(dateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var dateTime))
            throw new FormatException("Неверный формат даты");

        return new CLog(dateTime, level, message);
    }

    public List<CLog> ParseToList(IEnumerable<string> rawLogs)
    {
        var logs = new List<CLog>();
        foreach (var raw in rawLogs)
            try
            {
                logs.Add(Parse(raw));
            }
            catch (FormatException)
            {
                logs.Add(logs.Last());
            }

        return logs;
    }

    [GeneratedRegex(@"^\[(?<date>.+?)\]\s+\[(?<level>.+?)\]\s+(?<message>.+)$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}