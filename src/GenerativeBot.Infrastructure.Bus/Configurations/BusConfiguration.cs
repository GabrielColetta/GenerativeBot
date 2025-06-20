namespace GenerativeBot.Infrastructure.Bus.Configurations;

public class BusConfiguration
{
    public const string Key = "BusConfiguration";
    public string ConnectionString { get; set; } = string.Empty;

    public string QueueName { get; set; } = string.Empty;
}
