using Microsoft.Extensions.Hosting;

namespace GenerativeBot.Infrastructure.Bus.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddBusDependencies(this IHostApplicationBuilder builder)
    {
        builder.AddRabbitMQClient(connectionName: "messaging");
        return builder;
    }
}
