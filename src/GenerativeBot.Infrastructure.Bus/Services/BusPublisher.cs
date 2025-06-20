using GenerativeBot.Domain.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GenerativeBot.Infrastructure.Bus.Services;

public class BusPublisher : IBusPublisher
{
    private readonly IConnection _connection;

    public BusPublisher(IConnection connection)
    {
        _connection = connection;
    }

    public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        where TMessage : class
    {
        ArgumentNullException.ThrowIfNull(message);

        var queueName = $"{typeof(TMessage).Name}-queue";
        using var channel = _connection.CreateModel();
        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        await Task.Run(() =>
        {
            channel.BasicPublish(
                 exchange: string.Empty,
                 routingKey: queueName,
                 basicProperties: properties,
                 body: body);
        }, cancellationToken);
    }
}
