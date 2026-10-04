namespace Domain.Contracts.PositionContracts;

public record PositionTextAggregateDto(
    int AttributeDefinitionId,
    string Name,
    int TotalCount,
    List<PositionTextValueDto> TopValues
);
