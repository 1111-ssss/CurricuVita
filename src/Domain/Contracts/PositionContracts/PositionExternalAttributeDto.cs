namespace Domain.Contracts.PositionContracts;

public record PositionExternalAttributeDto(
    int AttributeDefinitionId,
    string Name,
    string Category,
    string DataType,
    bool IsRequired
);
