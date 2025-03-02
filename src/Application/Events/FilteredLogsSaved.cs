namespace Application.Events;

public record FilteredLogsSaved(string TargetTableName, List<CLog> Logs) : IApplicationEvent;