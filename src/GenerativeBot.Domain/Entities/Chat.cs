namespace GenerativeBot.Domain.Entities;

public class Chat
{
    public Chat(string role, string user, string content, float[] embedding)
    {
        Role = role;
        User = user;
        Content = content;
        Id = Guid.CreateVersion7();
        Date = DateTime.UtcNow;
        Embedding = embedding;
    }

    public Guid Id { get; set; }
    public string User { get; set; }
    public string Role { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
    public float[] Embedding { get; set; }
}
