using GenerativeBot.Domain.Interfaces;
using GenerativeBot.Domain.Models;
using Microsoft.Extensions.Hosting;

namespace GenerativeBot.Infrastructure.Bus.Services;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly IBusConsumer _busConsumer;

    public RabbitMqConsumerService(IBusConsumer busConsumer)
    {
        _busConsumer = busConsumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _busConsumer.ConsumeAsync<ChatQuery>();
    }
}
