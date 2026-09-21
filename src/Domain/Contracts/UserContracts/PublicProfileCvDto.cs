namespace Domain.Contracts.UserContracts;

public record PublicProfileCvDto(
    int CvId,
    int PositionId,
    string PositionTitle,
    DateTime UpdatedAt,
    int LikesCount
);
