
namespace GenerativeBot.Domain.Interfaces;

public interface IBusPublisher
{
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken) where TMessage : class;
}
