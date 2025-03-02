namespace Domain.Entities;

/// <summary>
///     Класс для представления сущности лога
/// </summary>
/// <param name="dateAndTime"></param>
/// <param name="levelOfImportance"></param>
/// <param name="message"></param>
public class CLog(DateTime dateAndTime, string? levelOfImportance, string? message)
{
    public DateTime DateAndTime { get; } = dateAndTime;
    public string? LevelOfImportance { get; } = levelOfImportance;
    public string? Message { get; } = message;

    public override string ToString()
    {
        return $"[{DateAndTime:yyyy-MM-dd HH:mm:ss}] [{LevelOfImportance}] {Message}";
    }
}