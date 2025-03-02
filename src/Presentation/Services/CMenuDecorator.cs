namespace Presentation.Services;

public class CMenuDecorator : CThemeColors
{
    /// <summary>
    ///     Стиль для текста в меню
    /// </summary>
    /// <returns></returns>
    protected Style InfoTextStyle()
    {
        return new Style(TextColor, decoration: Decoration.Italic);
    }

    /// <summary>
    ///     Стиль для текста лого в меню
    /// </summary>
    /// <returns></returns>
    protected Style LogoTextStyle()
    {
        return new Style(LogoTextColor, decoration: Decoration.Bold);
    }

    public Style HighlightTextStyle()
    {
        return new Style(HighlightColor, decoration: Decoration.Bold);
    }

    public Style OptionTextStyle()
    {
        return new Style(LogoTextColor, decoration: Decoration.Underline);
    }


    /// <summary>
    ///     Метод для создания текста логотипа
    /// </summary>
    /// <returns></returns>
    private Text LogoText()
    {
        return new Text("LogsAnalyzer", LogoTextStyle());
    }

    /// <summary>
    ///     Метод для создания обычного текста меню
    /// </summary>
    /// <returns></returns>
    private Text TextInPanel(string message)
    {
        return new Text(message, InfoTextStyle());
    }

    /// <summary>
    ///     Метод для создания шаблонной панели
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    protected Panel TemplateMenuPanel(string message)
    {
        var panel = new Panel(TextInPanel(message))
        {
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(BorderColor)
        };
        return panel;
    }

    /// <summary>
    ///     Метод для создания панели для логотипа в меню
    /// </summary>
    /// <returns></returns>
    protected Panel TemplateLogoMenuPanel()
    {
        var panel = new Panel(LogoText())
        {
            UseSafeBorder = true,
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(BorderColor)
        };
        return panel;
    }

    public void ShowText(string message)
    {
        AnsiConsole.MarkupLine($"[white]{message}[/]");
    }

    /// <summary>
    ///     Отображение текста в панели в консоли
    /// </summary>
    /// <param name="message"></param>
    public void ShowPanelText(string message)
    {
        ShowPanelMessageWithHeader(message);
    }

    /// <summary>
    ///     Отображение заголовка в панели в консоли
    /// </summary>
    /// <param name="message"></param>
    public void ShowTitle(string message)
    {
        ShowPanelMessageWithHeader(message, $"[{LogoTextColor}]Описание[/]");
    }

    /// <summary>
    ///     Отображение ошибки в панели в консоли
    /// </summary>
    /// <param name="errorMessage"></param>
    public void ShowError(string errorMessage)
    {
        ShowPanelMessageWithHeader(errorMessage, $"[{ErrorColor}]Ошибка[/]");
    }

    /// <summary>
    ///     Отображение успешной операции в панели в консоли
    /// </summary>
    /// <param name="operation"></param>
    public void ShowSuccess(string operation)
    {
        ShowPanelMessageWithHeader(operation, $"[{SuccessColor}]Успешно[/]");
    }

    /// <summary>
    ///     Отображение панели предупреждения в консоли
    /// </summary>
    /// <param name="warningMessage"></param>
    public void ShowWarning(string warningMessage)
    {
        ShowPanelMessageWithHeader(warningMessage, $"[{WarningColor}]Предупреждение[/]");
    }

    /// <summary>
    ///     Отображение панели лого в консоли
    /// </summary>
    public void ShowLogo()
    {
        var panel = TemplateLogoMenuPanel();
        AnsiConsole.Write(panel);
    }

    /// <summary>
    ///     Метод для отображения панели с заголовком в меню
    /// </summary>
    /// <param name="message"></param>
    /// <param name="header"></param>
    private void ShowPanelMessageWithHeader(string message, string? header = null)
    {
        var panel = TemplateMenuPanel(message);
        if (header != null) panel.Header = new PanelHeader(header, Justify.Left);
        AnsiConsole.Write(panel);
    }
}