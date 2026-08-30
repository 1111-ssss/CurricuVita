using Domain.Interfaces.Database;

namespace Domain.Entities;

public class Badge : IEntity
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SvgTemplate { get; set; } = string.Empty;
}