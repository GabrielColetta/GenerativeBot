namespace GenerativeBot.Infrastructure.Bus.Configurations;

public class BusConfiguration
{
    public const string Key = "Bus";
    public string QueueName { get; set; } = string.Empty;
}
