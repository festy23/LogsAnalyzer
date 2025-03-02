namespace Application.Events;

public record LogsCounted(CLogFilterDto Filter, int Count) : IApplicationEvent;