namespace GenerativeBot.AppHost;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var ravendb = builder
            .AddRavenDB("ravenServer")
            .WithDataVolume()
            .AddDatabase("ravendb");

        var rabbitmq = builder
            .AddRabbitMQ("messaging")
            .WithDataVolume(isReadOnly: false);

        var ollama = builder
            .AddOllama("ollama")
            .AddModel("gemma3:12b");

        builder
            .AddProject<Projects.GenerativeBot_Api>("api")
            .WithReference(rabbitmq)
            .WaitFor(rabbitmq)
            .WithReference(ravendb)
            .WaitFor(ravendb)
            .WithReference(ollama)
            .WaitFor(ollama);

        await builder.Build().RunAsync();
    }
}