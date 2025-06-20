using GenerativeBot.Domain.Entities;

namespace GenerativeBot.Domain.Interfaces;

public interface IChatRepository : IDbInsertable<Chat>, IDbVectorSearchable<Chat>
{
}
