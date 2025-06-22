using GenerativeBot.Domain.Interfaces;
using GenerativeBot.Infrastructure.Bus.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace GenerativeBot.Infrastructure.Bus.Services;

public class BusConsumer : IBusConsumer
{
    private readonly IConnection _connection;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _queueName;

    public BusConsumer(IConnection connection, IServiceScopeFactory serviceScopeFactory, IOptions<BusConfiguration> configuration)
    {
        _connection = connection;
        _queueName = configuration.Value.QueueName;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task ConsumeAsync<TMessage>()
        where TMessage : class
    {
        var queueName = string.Format(_queueName, typeof(TMessage).Name);

        using var serviceScope = _serviceScopeFactory.CreateScope();
        using var channel = _connection.CreateModel();

        channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        await Task.Run(() =>
        {
            channel.BasicConsume(queueName, autoAck: true, consumer: CreateEventHandler<TMessage>(serviceScope, channel));
        });
    }

    private AsyncEventingBasicConsumer CreateEventHandler<TMessage>(IServiceScope serviceScope, IModel channel)
        where TMessage : class
    {
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<TMessage>(body);

            var consumer = serviceScope.ServiceProvider.GetRequiredService<IConsumer<TMessage>>();
            await consumer.ConsumeAsync(message!);
        };

        return consumer;
    }
}
