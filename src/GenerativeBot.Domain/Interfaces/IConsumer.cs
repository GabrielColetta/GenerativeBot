namespace GenerativeBot.Domain.Interfaces;

public interface IConsumer<TMessage>
    where TMessage : class
{
    Task ConsumeAsync(TMessage message);
}
