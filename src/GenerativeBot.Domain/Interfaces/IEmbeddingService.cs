
namespace GenerativeBot.Domain.Interfaces;

public interface IEmbeddingService
{
    Task<float[]> GenerateVectorAsync(string contexts, CancellationToken cancellationToken);
}
