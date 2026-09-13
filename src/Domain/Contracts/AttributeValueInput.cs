namespace Domain.Contracts;

public record AttributeValueInput(
    int? ValueId,
    int AttributeDefinitionId,
    int? ExpectedVersion,
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
