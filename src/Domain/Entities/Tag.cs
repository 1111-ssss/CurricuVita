using Domain.Interfaces.Database;

namespace Domain.Entities;

public class Tag : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}