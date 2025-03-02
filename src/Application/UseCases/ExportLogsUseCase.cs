using System.Text;
using Application.Enums;

namespace Application.UseCases;
/// <summary>
/// Класс - реализация сценария использования "Экспорт логов"
/// </summary>
/// <param name="eventPublisher"></param>
/// <param name="fileWriteService"></param>
public class ExportLogsUseCase(IEventPublisher eventPublisher, IFileWriteService fileWriteService)
{
    public async Task Execute(CExportLogsDto dto)
    {
        var sb = new StringBuilder();

        foreach (var log in dto.Logs) sb.AppendLine(log.ToString());
        var fileContent = sb.ToString();

        switch (dto.ExportType)
        {
            case FileExportType.Write:
                await fileWriteService.WriteFileContentAsync(dto.FilePath, fileContent);
                break;
            case FileExportType.Append:
                await fileWriteService.AppendFileContentAsync(dto.FilePath, fileContent);
                break;
            case FileExportType.Delete:
                await fileWriteService.DeleteFileAsync(dto.FilePath);
                break;
            default:
                throw new Exception("Неверный тип операции работы с файлом");
        }

        await eventPublisher.PublishAsync(new LogsExported(dto.FilePath, dto.ExportType));
    }
}