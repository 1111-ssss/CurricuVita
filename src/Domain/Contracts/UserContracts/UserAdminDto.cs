namespace Domain.Contracts.UserContracts;

public record UserAdminDto(
    int Id,
    string Email,
    string FirstName,
    string LastName,
    List<string> Roles,
    bool IsLockedOut,
    DateTimeOffset? LockoutEnd,
    DateTime CreatedAt
);
