using Domain.Entities;
using Domain.ResultPattern.Result;

namespace Domain.Interfaces.Identity;

public interface IIdentityService
{
    Task<Result> ExternalLogin(
        string provider,
        string providerKey,
        string email,
        string? firstName = null,
        string? lastName = null,
        CancellationToken cancellationToken = default
    );
    Task<bool> IsEmailConfirmed(int userId);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserById(int id);
    Task<Result<int>> RegisterUser(
        string email,
        string firstName,
        string lastName,
        string password,
        CancellationToken cancellationToken = default
    );
    Task<Result<int>> Login(
        string email,
        string password,
        CancellationToken cancellationToken = default
    );
    Task Logout();
    Task<IList<string>> GetRoles(int userId);
    Task<Result<string>> GenerateEmailConfirmationToken(int userId);
    Task<Result> ConfirmEmail(int userId, string token);
}