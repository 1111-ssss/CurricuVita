using Domain.Enums;
using Domain.Interfaces.Database;

namespace Domain.Entities;

public class PositionAccessRule : IEntity
{
    public int Id { get; set; }
    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;

    public int AttributeDefinitionId { get; set; }
    public AttributeDefinition AttributeDefinition { get; set; } = null!;

    public Operator Operator { get; set; }
    public string Value { get; set; } = string.Empty;
}