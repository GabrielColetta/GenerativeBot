using GenerativeBot.Domain.Entities;
using Raven.Client.Documents.Indexes;

namespace GenerativeBot.Infrastructure.RavenDB.Configurations;

internal class ChatConfiguration : AbstractIndexCreationTask<Chat, ChatConfiguration.IndexEntry>
{
    public ChatConfiguration()
    {
        Map = chats =>
            from chat in chats
            select new IndexEntry
            {
                Embeddings = LoadVector("Embeddings", "id-for-task-open-ai")
            };

        SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
    }

    public class IndexEntry()
    {
        public object Embeddings { get; set; } = null!;
    }
}
