namespace Application.Mappings;

/// <summary>
///     Статический класс, который преобразует LogFilterDto в CLogFilter
/// </summary>
public static class LogFilterMapper
{
    /// <summary>
    ///     Преобразует LogFilterDto из слоя Application в CLogFilter из слоя Domain
    /// </summary>
    public static CLogFilter ToDomainFilter(CLogFilterDto dto)
    {
        return new CLogFilter(
            dto.Level,
            dto.StartDate,
            dto.EndDate,
            dto.Keyword,
            dto.CaseSensitive
        );
    }

    /// <summary>
    ///     Преобраузет CLogFilter в CLogFilterDto
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static CLogFilterDto ToDto(CLogFilter dto)
    {
        return new CLogFilterDto(
            dto.Level,
            dto.StartDate,
            dto.EndDate,
            dto.Keyword,
            dto.CaseSensitive
        );
    }
}