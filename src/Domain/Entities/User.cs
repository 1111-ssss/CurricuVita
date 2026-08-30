using Domain.Interfaces.Database;

namespace Domain.Entities;

public class User : IEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int Version { get; set; }
    public string? AvatarUrl { get; set; }

    public ICollection<UserBadge> Badges { get; set; } = new List<UserBadge>();
    public ICollection<UserAttributeValue> AttributeValues { get; set; } = new List<UserAttributeValue>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<CV> CVs { get; set; } = new List<CV>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
}