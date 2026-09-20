using Domain.Enums;
using Domain.Interfaces.Database;

namespace Domain.Entities;

public class CV : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;

    public CvStatus Status { get; set; } = CvStatus.Draft;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Version { get; set; }

    public ICollection<Like> Likes { get; set; } = new List<Like>();
}