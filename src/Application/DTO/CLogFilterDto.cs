namespace Application.DTO;
/// <summary>
/// DTO реализация CLogFilter
/// </summary>
/// <param name="Level"></param>
/// <param name="StartDate"></param>
/// <param name="EndDate"></param>
/// <param name="Keyword"></param>
/// <param name="CaseSensitive"></param>
public record CLogFilterDto(
    string? Level = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    string? Keyword = null,
    bool CaseSensitive = false
);