using Domain.Enums;

namespace Domain.Contracts.PositionContracts;

public record PositionAttributeDto(
    int AttributeDefinitionId,
    string Name,
    string Category,
    AttributeDataType DataType,
    bool IsRequired,
    int Order
);
