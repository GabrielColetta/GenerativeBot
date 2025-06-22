using GenerativeBot.Domain.Interfaces;
using GenerativeBot.Infrastructure.Http.Configurations;
using GenerativeBot.Infrastructure.Http.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace GenerativeBot.Infrastructure.Http.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var botConfiguration = configuration
            .GetSection(BotConfiguration.Key)
            .Get<BotConfiguration>()!;

        services
            .AddOptions<BotConfiguration>()
            .Configure(x =>
            {
                x = botConfiguration;
            });

        services.AddHttpClient<IDefaultHttpClient, DefaultHttpClient>()
            .ConfigureHttpClient(x => { 
                x.BaseAddress = new Uri($"{botConfiguration.BaseUrl}/bot{botConfiguration.Token}");
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(GetRetryPolicy());
        return services;
    }

    private static AsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}

