using Domain.Interfaces.Database;

namespace Domain.Entities;

public class DiscussionMessage : IEntity
{
    public int Id { get; set; }
    public int PositionId { get; set; }
    public Position Position { get; set; } = new();
    public int AuthorId { get; set; }
    public User Author { get; set; } = new();
    public string ContentMarkdown { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}