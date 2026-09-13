namespace Domain.Contracts.PositionContracts;

public record PositionCvListItemDto(
    int Id,
    int UserId,
    string CandidateName,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    int LikesCount
);
