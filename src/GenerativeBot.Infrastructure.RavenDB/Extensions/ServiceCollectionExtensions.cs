using GenerativeBot.Domain.Interfaces;
using GenerativeBot.Infrastructure.RavenDB.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GenerativeBot.Infrastructure.RavenDB.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseDependencies(this IServiceCollection services)
    {
        services.AddScoped<IChatRepository, ChatRepository>();
        return services;
    }
}
