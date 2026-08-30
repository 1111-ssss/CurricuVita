using Domain.Enums;
using Domain.Interfaces.Database;

namespace Domain.Entities;

public class AttributeDefinition : IEntity
{
    public int Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public AttributeDataType DataType { get; set; }
    public string? OptionsJson { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<PositionAttribute> PositionAttributes { get; set; } = new List<PositionAttribute>();
    public ICollection<UserAttributeValue> UserValues { get; set; } = new List<UserAttributeValue>();
}