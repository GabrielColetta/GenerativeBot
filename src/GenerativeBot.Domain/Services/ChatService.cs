using GenerativeBot.Domain.Entities;
using GenerativeBot.Domain.Interfaces;
using Microsoft.Extensions.AI;
using System.Runtime.CompilerServices;
using System.Text;

namespace GenerativeBot.Domain.Services;

public class ChatService : IChatService
{
    private readonly IChatClient _chatClient;
    private readonly IChatRepository _chatRepository;
    private readonly IEmbeddingService _embeddingService;

    public ChatService(IChatClient chatClient, IEmbeddingService embeddingService, IChatRepository chatRepository)
    {
        _chatClient = chatClient;
        _embeddingService = embeddingService;
        _chatRepository = chatRepository;
    }

    public async Task<string?> GenerateResponseAsync(string message, CancellationToken cancellationToken = default)
    {
        return await GenerateStreamingResponseAsync(message, cancellationToken).LastOrDefaultAsync(cancellationToken);
    }

    public async IAsyncEnumerable<string> GenerateStreamingResponseAsync(string message, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await UpdateChatHistoryAsync(ChatRole.User, message, cancellationToken);

        var chatHistory = await GetChatHistory(message, cancellationToken);

        var messageBuilder = new StringBuilder();
        await foreach (var response in _chatClient.GetStreamingResponseAsync(chatHistory, cancellationToken: cancellationToken))
        {
            if (response?.Contents != null)
            {
                messageBuilder.Append(response.Text);
                yield return messageBuilder.ToString();
            }
        }

        if (messageBuilder.Length > 0)
        {
            await UpdateChatHistoryAsync(ChatRole.Assistant, messageBuilder.ToString(), cancellationToken);
        }
    }

    private async Task<IEnumerable<ChatMessage>> GetChatHistory(string message, CancellationToken cancellationToken)
    {
        var response = await _chatRepository.VectorSearchAsync(Guid.NewGuid(), await _embeddingService.GenerateVectorAsync(message, cancellationToken), cancellationToken);
        if (response.Any())
        {
            return response.Select(x => new ChatMessage(new ChatRole(x.Role), x.Content));
        }
        return [new ChatMessage(ChatRole.User, message)];
    }

    private async Task UpdateChatHistoryAsync(ChatRole chatRole, string message, CancellationToken cancellationToken)
    {
        var chat = new Chat(chatRole.ToString(), message, message, await _embeddingService.GenerateVectorAsync(message, cancellationToken));
        await _chatRepository.InsertAsync(chat, CancellationToken.None);
    }
}
