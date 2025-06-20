using Microsoft.Extensions.Hosting;

namespace GenerativeBot.Infrastructure.RavenDB.Extensions;

public static class HostApplicationBuilder
{
    public static IHostApplicationBuilder AddDatabase(this IHostApplicationBuilder builder)
    {
        builder.AddRavenDBClient(connectionName: "ravendb", configureSettings:
        settings =>
        {
            settings.CreateDatabase = true;
            settings.DisableTracing = true;
        });
        return builder;
    }
}
