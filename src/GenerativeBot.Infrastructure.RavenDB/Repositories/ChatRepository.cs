using GenerativeBot.Domain.Entities;
using GenerativeBot.Domain.Interfaces;
using Raven.Client.Documents;

namespace GenerativeBot.Infrastructure.RavenDB.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly IDocumentStore _documentStore;

    public ChatRepository(IDocumentStore documentStore)
    {
        _documentStore = documentStore;
    }

    public async Task InsertAsync(Chat document, CancellationToken cancellationToken)
    {
        using var session = _documentStore.OpenAsyncSession();

        await session.StoreAsync(document, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Chat>> VectorSearchAsync(Guid id, float[] content, CancellationToken cancellationToken)
    {
        using var session = _documentStore.OpenAsyncSession();

        return await session
            .Query<Chat>()
            .Where(x => x.Id == id)
            .VectorSearch(
                field => field
                    .WithField(x => x.Embedding),
                searchTerm => searchTerm
                    .ByEmbedding(content),
                minimumSimilarity: 0.75f,
                numberOfCandidates: 20,
                isExact: true)
            .ToListAsync(cancellationToken);
    }
}
