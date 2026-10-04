namespace Domain.Contracts.PositionContracts;

public record PositionExternalExportDto(
    int PositionId,
    string Title,
    string? Company,
    string? Level,
    int PublishedCvCount,
    List<PositionExternalAttributeDto> Attributes,
    List<PositionNumericAggregateDto> NumericAggregates,
    List<PositionTextAggregateDto> TextAggregates
);
