namespace Domain.Contracts.UserContracts;

public record CvListItemDto(
    int Id,
    string PositionTitle,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
