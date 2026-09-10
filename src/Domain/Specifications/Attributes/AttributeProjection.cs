using Domain.Contracts;
using Domain.Enums;
using Domain.Helpers;

namespace Domain.Specifications.Attributes;

public record AttributeProjection(
    int Id,
    string Category,
    string Name,
    string Description,
    AttributeDataType DataType,
    string? OptionsJson,
    int Version,
    DateTime CreatedAt,
    int UsageCount
)
{
    public AttributeDto ToDto() => new(
        Id,
        Category,
        Name,
        Description,
        DataType,
        AttributeOptions.DeserializeOptions(OptionsJson),
        Version,
        CreatedAt,
        UsageCount);
}
