using Domain.Enums;

namespace Domain.Contracts;

public record AttributeDto(
    int Id,
    string Category,
    string Name,
    string Description,
    AttributeDataType DataType,
    List<string> Options,
    int Version,
    DateTime CreatedAt,
    int UsageCount
);
