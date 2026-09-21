namespace Domain.Contracts.PositionContracts;

public record PositionAggregatesDto(
    int PositionId,
    int PublishedCvCount,
    List<PositionNumericAggregateDto> NumericAggregates
);
