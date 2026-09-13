using Domain.Enums;

namespace Domain.Contracts;

public record ProfileAttributeValueDto(
    int? ValueId,
    int AttributeDefinitionId,
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
    string? DropdownValue
);
