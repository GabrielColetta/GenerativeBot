using GenerativeBot.AppHost.Configurations;
using GenerativeBot.Domain.Constants;
using Microsoft.Extensions.Configuration;

namespace GenerativeBot.AppHost;

internal class Program
{
    private const string DatabaseName = "ravenServer";
    private const string ApiProject = "api";

    private static async Task Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var languageModelConfiguration = builder.Configuration
            .GetSection(LanguageModelConfiguration.Key)
            .Get<LanguageModelConfiguration>()!;

        var ravendb = builder
            .AddRavenDB(DatabaseName)
            .WithDataVolume()
            .AddDatabase(GlobalConnectionName.Database);

        var rabbitmq = builder
            .AddRabbitMQ(GlobalConnectionName.Bus)
            .WithDataVolume(isReadOnly: false);

        var ollama = builder
            .AddOllama(GlobalConnectionName.AI)
            .WithGPUSupport()
            .AddModel(languageModelConfiguration.Model);

        builder
            .AddProject<Projects.GenerativeBot_Api>(ApiProject)
            .WithReference(rabbitmq)
            .WaitFor(rabbitmq)
            .WithReference(ravendb)
            .WaitFor(ravendb)
            .WithReference(ollama)
            .WaitFor(ollama);

        await builder.Build().RunAsync();
    }
}