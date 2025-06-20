

namespace GenerativeBot.Domain.Interfaces;

public interface IChatService
{
    Task<string?> GenerateResponseAsync(string message, CancellationToken cancellationToken = default);
    IAsyncEnumerable<string> GenerateStreamingResponseAsync(string message, CancellationToken cancellationToken = default);
}
