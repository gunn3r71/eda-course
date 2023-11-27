using MediatR;

namespace Telemetry.Common.EventBus;

public abstract class Event : IRequest
{
    protected Event()
    {
        Id = Guid.NewGuid();
        Name = GetType().Name;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
}