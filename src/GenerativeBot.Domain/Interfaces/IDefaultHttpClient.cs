

namespace GenerativeBot.Domain.Interfaces;

public interface IDefaultHttpClient
{
    Task PostAsync<TRequest>(string requestUri, TRequest body, CancellationToken cancellationToken = default)
        where TRequest : class;
}
