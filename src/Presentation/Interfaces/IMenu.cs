namespace Presentation.Interfaces;

/// <summary>
///     Интерфейс для реализаци меню
/// </summary>
public interface IMenu
{
    /// <summary>
    ///     Метод для запуска меню
    /// </summary>
    public void Run();

    /// <summary>
    ///     Метод для выхода из меню
    /// </summary>
    public void Exit();
}