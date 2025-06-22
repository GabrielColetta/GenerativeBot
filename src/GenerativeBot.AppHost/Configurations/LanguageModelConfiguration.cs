namespace GenerativeBot.AppHost.Configurations;

internal class LanguageModelConfiguration
{
    public const string Key = "LanguageModel";

    public string Model { get; set; } = string.Empty;
    public int Port { get; set; }
}
