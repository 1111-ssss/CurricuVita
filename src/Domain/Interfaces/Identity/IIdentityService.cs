using Domain.Entities;

namespace Domain.Interfaces.Identity;

public interface IIdentityService
{
    Task<(bool Succeeded, string? Error, int? UserId)> ExternalLoginAsync(
        string provider,
        string providerKey,
        string email,
        string? firstName,
        string? lastName,
        string? avatarUrl,
        CancellationToken cancellationToken = default
    );
    Task<bool> IsEmailConfirmedAsync(int userId);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByIdAsync(int id);
    Task<User> RegisterUserAsync(User user);
}