namespace Domain.Contracts.PositionContracts;

public record PositionListItemDto(
    int Id,
    string Title,
    string? Company,
    string? Level,
    bool IsPublic,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    int RequiredAttributesCount,
    int CvCount,
    List<string> Tags
);
