namespace Application.Interfaces.Events;

/// <summary>
///     Интерфейс диспетчера события TEvent
/// </summary>
public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IApplicationEvent;
}