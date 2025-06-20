using GenerativeBot.Domain.Extensions;
using GenerativeBot.Infrastructure.Bus.Extensions;
using GenerativeBot.Infrastructure.RavenDB.Extensions;

namespace GenerativeBot.Api;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults()
            .AddDatabase()
            .AddBusDependencies()
            .AddOllamaApiClient("ollama")
            .AddEmbeddingGenerator();

        builder.Services
            .AddProblemDetails()
            .AddDomainDependencies()
            .AddDatabaseDependencies()
            .AddBusDependencies();

        var app = builder.Build();

        app.UseExceptionHandler();
        app.MapDefaultEndpoints();

        await app.RunAsync();
    }
}