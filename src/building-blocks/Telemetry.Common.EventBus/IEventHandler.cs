using MediatR;

namespace Telemetry.Common.EventBus;

public interface IEventHandler<in T> : IRequestHandler<T> where T : Event
{
}