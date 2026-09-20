namespace Domain.Contracts.PositionContracts;

public record PositionNumericAggregateDto(
    int AttributeDefinitionId,
    string Name,
    int Count,
    double Average,
    decimal Min,
    decimal Max
);