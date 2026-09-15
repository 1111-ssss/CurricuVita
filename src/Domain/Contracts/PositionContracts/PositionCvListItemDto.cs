using Domain.Enums;

namespace Domain.Contracts.PositionContracts;

public record PositionCvListItemDto(
    int Id,
    int UserId,
    string CandidateName,
    CvStatus Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    int LikesCount
);
