using Domain.Enums;

namespace Domain.Contracts.CvContracts;

public record CvAttributeDto(
    int AttributeDefinitionId,
    int? ValueId,
    string Name,
    string Category,
    string Description,
    AttributeDataType DataType,
    List<string> Options,
    int? Version,
    string? StringValue,
    string? TextValue,
    string? ImageValue,
    decimal? NumericValue,
    DateTime? DateValue,
    DateTime? PeriodStartValue,
    DateTime? PeriodEndValue,
    bool? BooleanValue,
    string? DropdownValue,
    bool IsRequired,
    int Order,
    bool IsFilled
);