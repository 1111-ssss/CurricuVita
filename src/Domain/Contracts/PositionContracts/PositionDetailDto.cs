namespace Domain.Contracts.PositionContracts;

public record PositionDetailDto(
    int Id,
    string Title,
    string DescriptionMarkdown,
    string? Company,
    string? Level,
    bool IsPublic,
    int? MaxProjectCount,
    int Version,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<PositionAttributeDto> Attributes,
    List<PositionAccessRuleDto> AccessRules,
    List<string> Tags
);
