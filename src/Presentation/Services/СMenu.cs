using System.Globalization;

namespace Presentation.Services;

/// <summary>
///     Класс для реализации главного меню консольного приложения
/// </summary>
/// <param name="visualizer"></param>
/// <param name="importLogsUseCase"></param>
/// <param name="fileWriteService"></param>
/// <param name="fileReadService"></param>
/// <param name="filterLogsUseCase"></param>
/// <param name="saveFilteredLogsUseCase"></param>
/// <param name="clearLogsUseCase"></param>
/// <param name="exportLogsUseCase"></param>
/// <param name="logsRepository"></param>
/// <param name="menuDecorator"></param>
/// <param name="makeLogPlots"></param>
public class CMenu(
    ILogMenuVisualizer visualizer,
    ImportLogsUseCase importLogsUseCase,
    IFileWriteService fileWriteService,
    IFileReadService fileReadService,
    FilterLogsUseCase filterLogsUseCase,
    SaveFilteredLogsUseCase saveFilteredLogsUseCase,
    ClearLogsUseCase clearLogsUseCase,
    ExportLogsUseCase exportLogsUseCase,
    ILogsRepository logsRepository,
    CMenuDecorator menuDecorator,
    MakeLogPlotsUseCase makeLogPlots
)
    : IMenu
{
    private bool _isRunning = true;

    /// <summary>
    ///     <inheritdoc />
    /// </summary>
    public void Run()
    {
        while (_isRunning)
        {
            try
            {
                PrepareNewMenu("Добро пожаловать в главное меню анализатора логов!");

                var menuOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .HighlightStyle(menuDecorator.HighlightTextStyle())
                        .Title("[white]Выберите действие:[/]")
                        .AddChoices(
                            "Импорт из файла",
                            "Фильтрация логов с сохранением",
                            "Таблица логов",
                            "Календарь логов",
                            "Диаграмма логов",
                            "Графики логов",
                            "Экспорт в файл",
                            "Очистка всей БД логов",
                            "Завершение работы"
                        ));

                PrepareNewMenu(menuOption);
                ProcessMenuOption(menuOption);
            }
            catch (Exception ex)
            {
                menuDecorator.ShowError(ex.Message);
            }

            if (_isRunning)
                WaitForUserInput();
        }
    }

    /// <summary>
    ///     <inheritdoc />
    /// </summary>
    public void Exit()
    {
        _isRunning = false;
        menuDecorator.ShowText("Завершение работы приложения...");
    }

    /// <summary>
    ///     Метод для обработки опции меню
    /// </summary>
    /// <param name="choice"></param>
    private void ProcessMenuOption(string choice)
    {
        switch (choice)
        {
            case "Импорт из файла":
                ImportLogs();
                break;
            case "Фильтрация логов с сохранением":
                FilterAndSaveLogs();
                break;
            case "Таблица логов":
                ShowTableVisualization();
                break;
            case "Календарь логов":
                ShowCalendarVisualization();
                break;
            case "Диаграмма логов":
                ShowChartVisualization();
                break;
            case "Графики логов":
                MakeAndShowPlots();
                break;
            case "Экспорт в файл":
                ExportLogs();
                break;
            case "Очистка всей БД логов":
                ClearLogs();
                break;
            case "Завершение работы":
                Exit();
                break;
            default:
                menuDecorator.ShowError("Неверный выбор. Попробуйте снова.");
                break;
        }
    }

    /// <summary>
    ///     Метод для импорта логов в меню
    /// </summary>
    private void ImportLogs()
    {
        var filePath = AnsiConsole.Ask<string>("Введите путь к файлу логов:").Trim();
        if (string.IsNullOrEmpty(filePath))
        {
            menuDecorator.ShowError("Путь не может быть пустым.");
            return;
        }

        if (!fileReadService.DoesFileExists(filePath))
        {
            menuDecorator.ShowError("Файл не найден.");
            return;
        }

        importLogsUseCase.Execute(filePath).Wait();
        menuDecorator.ShowSuccess("Логи успешно импортированы!");
    }

    /// <summary>
    ///     Фильтрация и сохранения логов
    /// </summary>
    private void FilterAndSaveLogs()
    {
        var level = AnsiConsole.Prompt(
            new TextPrompt<string>("Введите уровень логов (или оставьте пустым):").AllowEmpty());
        var startDateStr = AnsiConsole.Prompt(
            new TextPrompt<string>("Введите дату начала (yyyy-MM-dd HH:mm:ss) (или оставьте пустым):").AllowEmpty());
        var endDateStr = AnsiConsole.Prompt(
            new TextPrompt<string>("Введите дату конца (yyyy-MM-dd HH:mm:ss) (или оставьте пустым):").AllowEmpty());
        var keyword = AnsiConsole.Prompt(
            new TextPrompt<string>("Введите ключевое слово (или оставьте пустым):").AllowEmpty());

        level = string.IsNullOrWhiteSpace(level) ? null : level;
        startDateStr = string.IsNullOrWhiteSpace(startDateStr) ? null : startDateStr;
        endDateStr = string.IsNullOrWhiteSpace(endDateStr) ? null : endDateStr;
        keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword;

        DateTime? startDate = null, endDate = null;
        if (!string.IsNullOrEmpty(startDateStr) &&
            DateTime.TryParseExact(startDateStr, "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out var sd))
            startDate = sd;
        if (!string.IsNullOrEmpty(endDateStr) &&
            DateTime.TryParseExact(endDateStr, "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out var ed))
            endDate = ed;

        var filterDto = new CLogFilterDto(level, startDate, endDate, keyword);
        var logs = filterLogsUseCase.Execute(filterDto).Result;
        visualizer.ShowTable(logs);

        var save = AnsiConsole.Confirm("Сохранить отфильтрованные логи?");
        if (!save) return;
        var targetName = AnsiConsole.Prompt(
            new TextPrompt<string>("Введите название для сохранения (оставьте пустым для 'ExportedLogs'):")
                .AllowEmpty());
        if (string.IsNullOrWhiteSpace(targetName))
            targetName = "ExportedLogs";

        saveFilteredLogsUseCase.Execute(filterDto, targetName).Wait();
        menuDecorator.ShowSuccess("Отфильтрованные логи успешно сохранены!");
    }

    /// <summary>
    ///     Отображение в консоли таблицы
    /// </summary>
    private void ShowTableVisualization()
    {
        var logs = logsRepository.GetLogs();
        visualizer.ShowTable(logs);
    }

    /// <summary>
    ///     Отображение календаря в консоли
    /// </summary>
    private void ShowCalendarVisualization()
    {
        var logs = logsRepository.GetLogs();
        visualizer.ShowCalendar(logs);
    }

    /// <summary>
    ///     Отображения диаграммы в консоли
    /// </summary>
    private void ShowChartVisualization()
    {
        var logs = logsRepository.GetLogs();
        visualizer.ShowChart(logs);
    }

    /// <summary>
    ///     Экспортирование логов в файл
    /// </summary>
    /// <exception cref="Exception"></exception>
    private void ExportLogs()
    {
        try
        {
            var filePath = AnsiConsole.Ask<string>("Введите путь для экспорта логов:").Trim();
            fileWriteService.ValidatePath(filePath);
            var opTypeStr = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .HighlightStyle(menuDecorator.HighlightTextStyle())
                    .Title("Выберите тип файловой операции:")
                    .AddChoices("Запись", "Добавление", "Удаление"));

            var opType = opTypeStr switch
            {
                "Запись" => FileExportType.Write,
                "Добавление" => FileExportType.Append,
                "Удаление" => FileExportType.Delete,
                _ => throw new Exception("Неверный тип операции")
            };

            var logs = logsRepository.GetLogs();

            if (logs.Count == 0) throw new Exception("Нет логов для экспорта");

            var exportDto = new CExportLogsDto(filePath, opType, logs);
            exportLogsUseCase.Execute(exportDto).Wait();
            menuDecorator.ShowSuccess("Логи экспортированы!");
        }
        catch (Exception ex)
        {
            menuDecorator.ShowError("Ошибка экспорта: " + ex.Message);
        }
    }

    /// <summary>
    ///     Очистка всех логов
    /// </summary>
    private void ClearLogs()
    {
        clearLogsUseCase.Execute().Wait();
        menuDecorator.ShowSuccess("Логи очищены!");
    }

    /// <summary>
    ///     Ожидание ввода пользователя - чтобы продолжить надо нажать любую кнопку
    /// </summary>
    private void WaitForUserInput()
    {
        menuDecorator.ShowText("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    /// <summary>
    ///     Подготовка к созданию нового меню
    /// </summary>
    /// <param name="description"></param>
    private void PrepareNewMenu(string description)
    {
        AnsiConsole.Clear();
        menuDecorator.ShowLogo();
        menuDecorator.ShowTitle(description);
    }

    /// <summary>
    ///     Создание и показ графиков
    /// </summary>
    private void MakeAndShowPlots()
    {
        var logCountPath = AnsiConsole
            .Ask<string>("Введите путь для сохранения графика количества логов (например, logcount.png):").Trim();
        var errorCountPath = AnsiConsole
            .Ask<string>("Введите путь для сохранения графика ошибок (например, errorcount.png):").Trim();

        if (string.IsNullOrEmpty(logCountPath)) logCountPath = "logcount.png";
        if (string.IsNullOrEmpty(errorCountPath)) errorCountPath = "errorcount.png";

        var result = makeLogPlots.ExecuteAsync(logCountPath, errorCountPath).Result;

        menuDecorator.ShowSuccess($"График количества логов сохранен: {result.LogCountPlotPath}\n" +
                                  $"График ошибок сохранен: {result.ErrorCountPlotPath}");
    }
}