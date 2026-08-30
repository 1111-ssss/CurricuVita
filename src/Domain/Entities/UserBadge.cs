using Domain.Interfaces.Database;

namespace Domain.Entities;

public class UserBadge : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = new();
    public int BadgeId { get; set; }
    public Badge Badge { get; set; } = new();
    public DateTime EarnedAt { get; set; }
}