using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Telemetry.Common.EventBus;
using Telemetry.Common.Events.TelemetryEvents;

EventBusConfiguration configuration = new EventBusConfiguration()
{
    Host = "localhost",
    Port = 5672,
    Password = "default_p@ss",
    Username = "lucas-eda-course",
    VHost = "/"
};

var dispatcher = new EventDispatcher(new Logger<Program>(new NullLoggerFactory()), configuration);

while (true)
{
    Console.Write("Decibels: ");
    int decibels = int.Parse(Console.ReadLine() ?? string.Empty);

    NoiseEvent @event = new NoiseEvent(decibels);

    dispatcher.Send(@event);
}