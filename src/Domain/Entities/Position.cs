using Domain.Interfaces.Database;

namespace Domain.Entities;

public class Position : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string DescriptionMarkdown { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public int? MaxProjectCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Version { get; set; }
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; } = new();

    public ICollection<PositionAttribute> RequiredAttributes { get; set; } = new List<PositionAttribute>();
    public ICollection<PositionTag> RequiredTags { get; set; } = new List<PositionTag>();
    public ICollection<PositionAccessRule> AccessRules { get; set; } = new List<PositionAccessRule>();
    public ICollection<CV> CVs { get; set; } = new List<CV>();
    public ICollection<DiscussionMessage> Messages { get; set; } = new List<DiscussionMessage>();
}