using Application.Enums;

namespace Application.Events;

/// <summary>
///     Событие - экспорт логов
/// </summary>
/// <param name="FilePath"></param>
/// <param name="ExportType"></param>
public record LogsExported(string FilePath, FileExportType ExportType) : IApplicationEvent;