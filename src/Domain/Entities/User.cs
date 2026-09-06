using Domain.Interfaces.Database;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<int>, IEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int Version { get; set; }
    public string? AvatarUrl { get; set; }
    public string? AvatarPublicId { get; set; }

    public ICollection<UserBadge> Badges { get; set; } = new List<UserBadge>();
    public ICollection<UserAttributeValue> AttributeValues { get; set; } = new List<UserAttributeValue>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<CV> CVs { get; set; } = new List<CV>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
}