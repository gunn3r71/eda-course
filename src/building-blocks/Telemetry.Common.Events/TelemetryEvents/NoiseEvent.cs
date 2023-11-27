using Telemetry.Common.EventBus;

namespace Telemetry.Common.Events.TelemetryEvents;

public class NoiseEvent : Event
{
    public NoiseEvent(int decibels)
    {
        Decibels = decibels;
    }

    public int Decibels { get; private set; }
}