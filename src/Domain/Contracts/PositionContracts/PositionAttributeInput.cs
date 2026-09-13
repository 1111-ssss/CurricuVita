namespace Domain.Contracts.PositionContracts;

public record PositionAttributeInput(
    int AttributeDefinitionId,
    bool IsRequired
);
