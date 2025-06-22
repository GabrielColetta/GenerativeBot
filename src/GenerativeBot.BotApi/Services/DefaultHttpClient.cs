using GenerativeBot.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenerativeBot.Infrastructure.Http.Services;

public class DefaultHttpClient : IDefaultHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DefaultHttpClient> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        AllowOutOfOrderMetadataProperties = true
    };

    public DefaultHttpClient(HttpClient httpClient, ILogger<DefaultHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task PostAsync<TRequest>(string requestUri, TRequest body, CancellationToken cancellationToken = default) 
        where TRequest : class
    {
        await SendRequestAsync(HttpMethod.Post, requestUri, body, cancellationToken); 
    }

    private async Task SendRequestAsync<TRequest>(HttpMethod httpMethod, string requestUri, TRequest body, CancellationToken cancellationToken = default)
        where TRequest : class
    {
        ArgumentNullException
            .ThrowIfNull(body);

        var uriBuilder = new UriBuilder(_httpClient.BaseAddress!)
        {
            Path = requestUri
        };

        var httpContent = JsonContent.Create(body, body.GetType(), options: _jsonSerializerOptions);
        var httpRequest = new HttpRequestMessage(httpMethod, uriBuilder.Uri)
        {
            Content = httpContent
        };

        HttpResponseMessage httpResponse;
        try
        {
            httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);
        }
        catch (TaskCanceledException exception)
        {
            _logger.LogError(exception, "Maximum time reached, details: {Message}", exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unexpected error: {Message}", exception.Message);
        }
    }
}
