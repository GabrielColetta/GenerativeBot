using GenerativeBot.Domain.Constants;
using GenerativeBot.Domain.Extensions;
using GenerativeBot.Infrastructure.Bus.Extensions;
using GenerativeBot.Infrastructure.Http.Extensions;
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
            .AddOllamaApiClient(GlobalConnectionName.AI)
            .AddEmbeddingGenerator();

        builder.Services
            .AddProblemDetails()
            .AddDomainDependencies()
            .AddDatabaseDependencies()
            .AddBusDependencies()
            .AddCustomHttpClients(builder.Configuration);

        var app = builder.Build();

        app.UseExceptionHandler();
        app.MapDefaultEndpoints();

        await app.RunAsync();
    }
}