namespace Domain.Contracts.HomeContracts;

public record HomePositionItemDto(
    int Id,
    string Title,
    string? Company,
    string? Level,
    bool IsPublic,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    int CvCount
);