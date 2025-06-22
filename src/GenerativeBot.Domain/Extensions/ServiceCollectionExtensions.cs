using GenerativeBot.Domain.Consumers;
using GenerativeBot.Domain.Interfaces;
using GenerativeBot.Domain.Models;
using GenerativeBot.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GenerativeBot.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainDependencies(this IServiceCollection services)
    {
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IConsumer<ChatQuery>, ChatConsumer>();
        return services;
    }
}
