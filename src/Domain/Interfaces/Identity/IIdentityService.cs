using Domain.Entities;
using Domain.ResultPattern.Result;

namespace Domain.Interfaces.Identity;

public interface IIdentityService
{
    Task<Result<int>> ExternalLoginAsync(
        string provider,
        string providerKey,
        string email,
        string? firstName,
        string? lastName,
        string? password,
        CancellationToken cancellationToken = default
    );
    Task<bool> IsEmailConfirmedAsync(int userId);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(int id);
    Task<Result<int>> RegisterUserAsync(
        string email,
        string firstName,
        string lastName,
        string password,
        CancellationToken cancellationToken = default
    );
}