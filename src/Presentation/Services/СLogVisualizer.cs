using System.Diagnostics;
using DateTime = System.DateTime;

namespace Presentation.Services;

/// <summary>
///     Реализация ILogVisualizer для консольного приложения.
///     Отвечает за визуализацию логов в виде таблицы, диаграммы разбивки и календаря.
/// </summary>
public class CLogVisualizer(ILogsRepository logsRepository)
    : CThemeColors, ILogMenuVisualizer
{
    public void ShowVisualization()
    {
        var logs = logsRepository.GetLogs();

        ShowTable(logs);

        ShowChart(logs);

        ShowCalendar(logs);
    }

    public void ShowTable(List<CLog> logs)
    {
        var table = new Table
        {
            Border = TableBorder.Rounded,
            BorderStyle = new Style(HighlightColor)
        };

        table.AddColumn("Дата и время");
        table.AddColumn("Уровень");
        table.AddColumn("Сообщение");

        foreach (var log in logs)
            table.AddRow(
                log.DateAndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                log.LevelOfImportance ?? string.Empty,
                log.Message ?? string.Empty);

        AnsiConsole.Write(table);
    }

    public void ShowChart(List<CLog> logs)
    {
        var levelGroups = logs.GroupBy(l => l.LevelOfImportance)
            .Select(g => new { Level = g.Key, Count = g.Count() })
            .ToList();

        var chart = new BarChart()
            .Width(60)
            .Label($"[{HighlightColor}]Диаграмма разбивки по уровням логов[/]");

        foreach (var group in levelGroups)
        {
            Debug.Assert(group.Level != null, "group.Level != null");
            chart.AddItem(group.Level, group.Count, TextColor);
        }

        AnsiConsole.Write(chart);
    }

    public void ShowCalendar(List<CLog> logs)
    {
        var now = DateTime.Now;
        var currentYear = now.Year;
        var currentMonth = now.Month;
        var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

        var logCounts = logs
            .Where(l => l.DateAndTime.Year == currentYear && l.DateAndTime.Month == currentMonth)
            .GroupBy(l => l.DateAndTime.Day)
            .ToDictionary(g => g.Key, g => g.Count());

        var table = new Table
        {
            Border = TableBorder.Rounded,
            BorderStyle = new Style(HighlightColor)
        };
        table.AddColumn("Пн");
        table.AddColumn("Вт");
        table.AddColumn("Ср");
        table.AddColumn("Чт");
        table.AddColumn("Пт");
        table.AddColumn("Сб");
        table.AddColumn("Вс");

        var firstDay = new DateTime(currentYear, currentMonth, 1);
        var startDayIndex = ((int)firstDay.DayOfWeek + 6) % 7; // Преобразуем: Пн=0, Вт=1, ..., Вс=6

        var day = 1;
        while (day <= daysInMonth)
        {
            var row = new List<string>();
            for (var i = 0; i < 7; i++)
                if (day == 1 && i < startDayIndex)
                {
                    row.Add(""); 
                }
                else if (day > daysInMonth)
                {
                    row.Add("");
                }
                else
                {
                    row.Add(logCounts.TryGetValue(day, out var count)
                        ? $"[bold yellow]{day} ({count})[/]"
                        : day.ToString());


                    day++;
                }

            table.AddRow(row.ToArray());
        }

        AnsiConsole.Write(table);
    }
}