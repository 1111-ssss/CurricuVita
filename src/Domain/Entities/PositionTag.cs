using Domain.Interfaces.Database;

namespace Domain.Entities;

public class PositionTag : IEntity
{
    public int Id { get; set; }
    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}