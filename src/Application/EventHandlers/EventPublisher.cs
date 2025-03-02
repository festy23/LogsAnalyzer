using Microsoft.Extensions.DependencyInjection;

namespace Application.EventHandlers;

public class EventPublisher(IServiceProvider serviceProvider) : IEventPublisher
{
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IApplicationEvent
    {
        var handlers = serviceProvider.GetServices<IEventHandler<TEvent>>();
        foreach (var handler in handlers) await handler.HandleAsync(@event);
    }
}