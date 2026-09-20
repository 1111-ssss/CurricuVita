using Domain.Enums;

namespace Domain.Contracts.UserContracts;

public record CvListItemDto(
    int Id,
    int PositionId,
    string PositionTitle,
    CvStatus Status,
    bool IsHidden,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
