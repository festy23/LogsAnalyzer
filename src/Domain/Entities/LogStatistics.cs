namespace Domain.Entities;
/// <summary>
/// Статистика логов в программе
/// </summary>
public class LogStatistics
{   
    /// <summary>
    /// Общее количество логов
    /// </summary>
    public int TotalMessages { get; set; }   
    /// <summary>
    /// Количество логов по уровням
    /// </summary>
    public Dictionary<string, int>? LogLevelsCount { get; set; } 
}