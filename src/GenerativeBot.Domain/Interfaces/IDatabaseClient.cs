namespace GenerativeBot.Domain.Interfaces;

public interface IDbInsertable<TDocument>
    where TDocument : class
{
    Task InsertAsync(TDocument document, CancellationToken cancellationToken);
}
