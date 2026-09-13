using Domain.Contracts.PositionContracts;

namespace Domain.Specifications.Positions;

public record PositionListProjection(
    int Id,
    string Title,
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
        IsPublic,
        CreatedAt,
        UpdatedAt,
        RequiredAttributesCount,
        CvCount,
        Tags
    );
}
