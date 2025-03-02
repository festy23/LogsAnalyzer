namespace Application.Interfaces.Events;

/// <summary>
///     Интерфейс - обработчик события TEvent
/// </summary>
/// <typeparam name="TEvent">Тип события</typeparam>
public interface IEventHandler<in TEvent> where TEvent : class
{
    Task HandleAsync(TEvent @event);
}