namespace GenerativeBot.Domain.Interfaces;

public interface IDbVectorSearchable<TEntity>
    where TEntity : class
{
    Task<IEnumerable<TEntity>> VectorSearchAsync(Guid id, float[] content, CancellationToken cancellationToken);
}
