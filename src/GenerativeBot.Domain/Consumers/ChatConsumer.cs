using GenerativeBot.Domain.Interfaces;
using GenerativeBot.Domain.Models;
using Microsoft.Extensions.Logging;

namespace GenerativeBot.Domain.Consumers;

public class ChatConsumer : IConsumer<ChatQuery>
{
    private const string RequestUri = "sendMessage";

    private readonly IChatService _chatService;
    private readonly IDefaultHttpClient _defaultHttpClient;
    private readonly ILogger<ChatConsumer> _logger;

    public ChatConsumer(IChatService chatService, IDefaultHttpClient defaultHttpClient, ILogger<ChatConsumer> logger)
    {
        _chatService = chatService;
        _defaultHttpClient = defaultHttpClient;
        _logger = logger;
    }

    public async Task ConsumeAsync(ChatQuery message)
    {
        var response = await _chatService.GenerateResponseAsync(message.Content);

        if (response is not null)
        {
            await _defaultHttpClient.PostAsync(RequestUri, response);
        }

        _logger.LogWarning("Bot does not made any response");
    }
}
