using Application.Interfaces.Events;

namespace Infrastructure.Events;

/// <summary>
///     Класс - реализация IEventPublisher.
/// </summary>
public class EventPublisher : IEventPublisher
{
    private readonly IDictionary<Type, List<object>> _handlers = new Dictionary<Type, List<object>>();

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IApplicationEvent
    {
        var eventType = typeof(TEvent);
        if (_handlers.TryGetValue(eventType, out var handler1))
            foreach (var handlerObj in handler1)
                if (handlerObj is IEventHandler<TEvent> handler)
                    await handler.HandleAsync(@event);
    }

    public void Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : class, IApplicationEvent
    {
        var eventType = typeof(TEvent);
        if (!_handlers.ContainsKey(eventType)) _handlers[eventType] = [];
        _handlers[eventType].Add(handler);
    }
}