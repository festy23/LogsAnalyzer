namespace Application.Mappings;

public static class LogMapper
{
    /// <summary>
    ///     Преобразует CLogFilterDto из слоя Application в CLogFilter из слоя Domain
    /// </summary>
    public static CLog ToDomain(CLogDto dto)
    {
        return new CLog(dto.DateAndTime, dto.Level, dto.Message);
    }

    /// <summary>
    ///     Преобразует из Clog в CLogDto
    /// </summary>
    /// <param name="log"></param>
    /// <returns></returns>
    public static CLogDto ToDto(CLog log)
    {
        return new CLogDto(log.DateAndTime, log.LevelOfImportance, log.Message);
    }
}