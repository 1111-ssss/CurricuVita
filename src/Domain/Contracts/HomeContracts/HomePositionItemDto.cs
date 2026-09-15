namespace Domain.Contracts.HomeContracts;

public record HomePositionItemDto(
    int Id,
    string Title,
    bool IsPublic,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    int CvCount
);