namespace Domain.Interfaces.Identity;

public interface ICurrentUserService
{
    int? UserId { get; }
    string? Email { get; }
    string? GetBaseUrl();
}