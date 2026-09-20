using Domain.Contracts.PositionContracts;

namespace Domain.Specifications.Positions;

public record PositionListProjection(
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
)
{
    public PositionListItemDto ToDto() => new(
        Id,
        Title,
        Company,
        Level,
        IsPublic,
        CreatedAt,
        UpdatedAt,
        RequiredAttributesCount,
        CvCount,
        Tags
    );
}
