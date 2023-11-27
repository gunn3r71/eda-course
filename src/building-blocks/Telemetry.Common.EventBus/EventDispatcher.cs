using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Telemetry.Common.EventBus;

public class EventDispatcher : IEventDispatcher
{
    private readonly ILogger _logger;
    private readonly ConnectionFactory _connectionFactory;

    public EventDispatcher(ILogger logger, EventBusConfiguration eventBusConfiguration)
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
    
    public void Send(Event @event)
    {
        string routingKey = nameof(@event);
        
        _logger.LogTrace("Sending event to {0} queue", routingKey);
        
        if (@event is null)
            throw new ArgumentNullException(nameof(@event), "Event cannot be null");

        string serializedEvent = SerializeEvent(@event);
        
        _logger.LogTrace("Serialized event: {0}", serializedEvent);

        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        
        DeclareQueue(channel, routingKey);
        
        channel.BasicPublish(exchange: "",
                             routingKey: routingKey,
                             body: Encoding.UTF8.GetBytes(serializedEvent));
        
        _logger.LogTrace("Sent {0}", @event);
    }
    
    private string SerializeEvent(Event @event) => 
        JsonConvert.SerializeObject(@event);

    private void DeclareQueue(IModel model, string name)
    {
        model?.QueueDeclare(queue: name,
                            autoDelete: false);
    }
}