using Domain.Interfaces.Database;

namespace Domain.Entities;

public class ProjectTag : IEntity
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; } = new();
    public int TagId { get; set; }
    public Tag Tag { get; set; } = new();
}