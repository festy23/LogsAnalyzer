using Application.Enums;

namespace Application.DTO;
/// <summary>
/// DTO реализцаия CExportLogs
/// </summary>
/// <param name="FilePath"></param>
/// <param name="ExportType"></param>
/// <param name="Logs"></param>
public record CExportLogsDto(
    string FilePath,
    FileExportType ExportType,
    List<CLog> Logs
);