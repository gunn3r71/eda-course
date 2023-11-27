namespace Telemetry.Common.EventBus;

public interface IEventDispatcher
{
    void Send(Event @event);
}