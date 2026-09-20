using Domain.Interfaces.Database;

namespace Domain.Entities;

public class Tag : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<ProjectTag> ProjectTags { get; set; } = new List<ProjectTag>();
    public ICollection<PositionTag> PositionTags { get; set; } = new List<PositionTag>();
}