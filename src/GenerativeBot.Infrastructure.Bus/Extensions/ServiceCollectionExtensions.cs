using GenerativeBot.Domain.Interfaces;
using GenerativeBot.Infrastructure.Bus.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GenerativeBot.Infrastructure.Bus.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IBusPublisher, BusPublisher>();
        services.AddHostedService<RabbitMqConsumerService>();
        return services;
    }
}
