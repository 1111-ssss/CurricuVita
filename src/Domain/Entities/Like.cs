using Domain.Interfaces.Database;

namespace Domain.Entities;

public class Like : IEntity
{
    public int Id { get; set; }
    public int CVId { get; set; }
    public CV CV { get; set; } = new();
    public int RecruiterId { get; set; }
    public User Recruiter { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}