namespace Application.Events;

public record LogsImported(List<CLog> Logs) : IApplicationEvent;