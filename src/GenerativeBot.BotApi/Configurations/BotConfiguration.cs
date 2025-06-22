using System.Globalization;

namespace GenerativeBot.Infrastructure.Http.Configurations;

public class BotConfiguration
{
    public const string Key = "Bot";

    public string BaseUrl { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public void ValidateToken()
    {
        int index = Token.IndexOf(':');
        if (index is < 1 or > 16 || !long.TryParse(Token[..index], NumberStyles.Integer, CultureInfo.InvariantCulture, out long _))
        {
            throw new ArgumentException("Bot token invalid", nameof(Token));
        }
    }
}
