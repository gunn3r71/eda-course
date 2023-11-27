using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Telemetry.Common.EventBus;

public class EventConsumer : IEventConsumer
{
    private readonly ILogger _logger;
    private readonly ConnectionFactory _connectionFactory;

    public EventConsumer(ILogger logger, EventBusConfiguration eventBusConfiguration)
    {
        _logger = logger;
        _connectionFactory = new ConnectionFactory()
        {
            HostName = eventBusConfiguration.Host,
            VirtualHost = eventBusConfiguration.VHost,
            Port = eventBusConfiguration.Port,
            UserName = eventBusConfiguration.Username,
            Password = eventBusConfiguration.Password
        };
    }
    
    public void RegisterConsumer<T>(IEventHandler<T> handler) where T : Event
    {
        string routingKey = nameof(T);
        
        _logger.LogTrace("Receiving event from {0} queue", routingKey);

        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(routingKey,
                             true,
                             true,
                             false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (_, eventArgs) =>
        {
            byte[]? body = eventArgs?.Body.ToArray();

            if (body is null)
                throw new ArgumentNullException(nameof(body));

            var @event = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));

            if (@event is null)
                throw new ArgumentNullException(nameof(@event));
            
            await handler.Handle(@event, CancellationToken.None);
        };
    }
}