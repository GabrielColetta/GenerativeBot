using GenerativeBot.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace GenerativeBot.Infrastructure.Bus.Services;

public class BusConsumer : IBusConsumer
{
    private readonly IConnection _connection;

    public BusConsumer(IConnection connection)
    {
        _connection = connection;
    }

    public async Task ConsumeAsync<TMessage>()
        where TMessage : class
    {
        using var channel = _connection.CreateModel();

        var queueName = $"{typeof(TMessage).Name}-queue";
        channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            return Task.CompletedTask;
        };

        await Task.Run(() =>
        {
            channel.BasicConsume(queueName, autoAck: true, consumer: consumer);
        });
    }
}
