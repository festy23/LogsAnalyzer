namespace Application.DTO;
/// <summary>
/// DTO релизация CLog
/// </summary>
/// <param name="dateAndTime"></param>
/// <param name="level"></param>
/// <param name="message"></param>
public class CLogDto(DateTime dateAndTime, string? level, string? message)
{
    public DateTime DateAndTime { get; set; } = dateAndTime; // Форматируем дату как строку
    public string? Level { get; set; } = level;
    public string? Message { get; set; } = message;
}