namespace Application.Events;

public record LogsFiltered(CLogFilterDto FilterDto, List<CLog> FilteredLogs) : IApplicationEvent;