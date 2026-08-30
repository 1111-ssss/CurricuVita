using Domain.Interfaces.Database;

namespace Domain.Entities;

public class CV : IEntity
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = new();

    public int PositionId { get; set; }
    public Position Position { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Version { get; set; }

    public ICollection<Like> Likes { get; set; } = new List<Like>();
}