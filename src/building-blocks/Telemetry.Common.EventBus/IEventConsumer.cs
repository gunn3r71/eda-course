namespace Telemetry.Common.EventBus;

public interface IEventConsumer
{
    void RegisterConsumer<T>(IEventHandler<T> handler) where T : Event;
}