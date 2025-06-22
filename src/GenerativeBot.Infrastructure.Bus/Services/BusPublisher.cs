using GenerativeBot.Domain.Interfaces;
using GenerativeBot.Infrastructure.Bus.Configurations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GenerativeBot.Infrastructure.Bus.Services;

public class BusPublisher : IBusPublisher
{
    private readonly IConnection _connection;
    private readonly string _queueName;

    public BusPublisher(IConnection connection, IOptions<BusConfiguration> configuration)
    {
        _connection = connection;
        _queueName = configuration.Value.QueueName;
    }

    public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        where TMessage : class
    {
        ArgumentNullException.ThrowIfNull(message);

        var queueName = string.Format(_queueName, typeof(TMessage).Name);

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
