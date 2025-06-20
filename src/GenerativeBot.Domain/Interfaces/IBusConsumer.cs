
namespace GenerativeBot.Domain.Interfaces;

public interface IBusConsumer
{
    Task ConsumeAsync<TMessage>() where TMessage : class;
}
