using GenerativeBot.Domain.Interfaces;
using Microsoft.Extensions.AI;

namespace GenerativeBot.Domain.Services;

public class EmbeddingService : IEmbeddingService
{
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;

    public EmbeddingService(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
    {
        _embeddingGenerator = embeddingGenerator;
    }

    public async Task<float[]> GenerateVectorAsync(string contexts, CancellationToken cancellationToken)
    {
        var response = await _embeddingGenerator.GenerateVectorAsync(contexts, cancellationToken: cancellationToken);

        return response.ToArray();
    }
}
