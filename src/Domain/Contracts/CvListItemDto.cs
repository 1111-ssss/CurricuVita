namespace Domain.Contracts;

public record CvListItemDto(
    int Id,
    string PositionTitle,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
