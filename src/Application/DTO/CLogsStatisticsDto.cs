namespace Application.DTO;
/// <summary>
/// DTO реализация CLogsStatistics
/// </summary>
public record CLogsStatisticsDto(DateTime? DtoFrom, DateTime? DtoTo) : IApplicationEvent;