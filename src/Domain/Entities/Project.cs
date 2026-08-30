using Domain.Interfaces.Database;

namespace Domain.Entities;

public class Project : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = new();
    public string Location { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string DescriptionMarkdown { get; set; } = string.Empty;

    public ICollection<ProjectTag> Tags { get; set; } = new List<ProjectTag>();
}