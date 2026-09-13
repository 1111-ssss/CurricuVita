namespace Domain.Contracts;

public record MeDto(
    string FirstName,
    string LastName,
    string Location,
    string? AvatarUrl,
    int Version
);
