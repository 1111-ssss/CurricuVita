using Domain.Interfaces.Database;

namespace Domain.Entities;

public class UserAttributeValue : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = new();

    public int AttributeDefinitionId { get; set; }
    public AttributeDefinition AttributeDefinition { get; set; } = new();

    public string? StringValue { get; set; }
    public string? TextValue { get; set; }
    public string? ImageValue { get; set; }
    public decimal? NumericValue { get; set; }
    public DateTime? DateValue { get; set; }
    public DateTime? PeriodStartValue { get; set; }
    public DateTime? PeriodEndValue { get; set; }
    public bool? BooleanValue { get; set; }
    
    public int Version { get; set; }
    public DateTime UpdatedAt { get; set; }
}