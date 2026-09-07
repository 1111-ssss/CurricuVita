using Domain.Entities;
using Domain.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Domain.ResultPattern.Result;
using Domain.ResultPattern.Errors;
using Infrastructure.Constants;
using Microsoft.Extensions.Options;
using Domain.Options;

namespace Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly EmailSenderOptions _options;

    public IdentityService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IOptions<EmailSenderOptions> options
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _options = options.Value;
    }

    public async Task<Result> ExternalLogin(
        string provider,
        string providerKey,
        string email,
        string? firstName = null,
        string? lastName = null,
        CancellationToken cancellationToken = default
    )
    {
        var loginInfo = new UserLoginInfo(provider, providerKey, provider);

        var user = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
        if (user is not null)
        {
            UpdateUserProfile(user, firstName, lastName);
            await _userManager.UpdateAsync(user);

            await _signInManager.SignInAsync(user, isPersistent: IdentityServiceConstants.ExternalLoginIsPersistent);
            return Result.Success();
        }

        user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName ?? string.Empty,
                LastName = lastName ?? string.Empty,
                Location = string.Empty,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Version = 1
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                return Result.Failure(Errors.ExternalLoginError with { Message = GetErrorsText(createResult) });
            }
        }
        else
        {
            UpdateUserProfile(user, firstName, lastName);
            await _userManager.UpdateAsync(user);
        }

        var addLoginResult = await _userManager.AddLoginAsync(user, loginInfo);
        if (!addLoginResult.Succeeded)
        {
            return Result.Failure(Errors.ExternalLoginError with { Message = GetErrorsText(addLoginResult) });
        }

        await _signInManager.SignInAsync(user, isPersistent: true);
        return Result.Success();
    }

    public async Task<bool> IsEmailConfirmed(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user?.EmailConfirmed ?? false;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<User?> GetUserById(int userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<Result<int>> RegisterUser(
        string email,
        string firstName,
        string lastName,
        string password,
        CancellationToken cancellationToken = default
    )
    {
        var user = new User
        {
            UserName = email,
            Email = email,
            EmailConfirmed = false,
            FirstName = firstName,
            LastName = lastName,
            Location = string.Empty,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Version = 1
        };

        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            return Result.Failure(Errors.ExternalLoginError with { Message = GetErrorsText(createResult) });
        }

        return Result<int>.Success(user.Id);
    }

    public async Task<Result<int>> Login(
        string email,
        string password,
        CancellationToken cancellationToken = default
    )
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.Failure(Errors.InvalidCredentials);
        }

        if (!user.EmailConfirmed)
        {
            return Result.Failure(Errors.EmailNotConfirmed);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user,
            password,
            isPersistent: true,
            lockoutOnFailure: true
        );

        if (result.Succeeded)
        {
            return Result<int>.Success(user.Id);
        }

        if (result.IsLockedOut)
        {
            return Result.Failure(Errors.UserLockedOut);
        }

        if (result.IsNotAllowed)
        {
            return Result.Failure(Errors.LoginNotAllowed);
        }

        return Result.Failure(Errors.InvalidCredentials);
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IList<string>> GetRoles(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return new List<string>();
        }

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<Result<string>> GenerateEmailConfirmationToken(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure(Errors.UserNotFound);
        }
        if (user.EmailConfirmationTokenSentAt is not null
            && DateTime.UtcNow - user.EmailConfirmationTokenSentAt.Value < _options.TokenLifetime)
        {
            return Result.Failure(Errors.EmailConfirmationTokenAlreadySent);
        }

        user.EmailConfirmationTokenSentAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return Result<string>.Success(token);
    }

    public async Task<Result> ConfirmEmail(int userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure(Errors.UserNotFound);
        }

        await _userManager.ConfirmEmailAsync(user, token);
        return Result.Success();
    }

    private static void UpdateUserProfile(User user, string? firstName, string? lastName)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            user.FirstName = firstName;
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            user.LastName = lastName;
        }

        user.UpdatedAt = DateTime.UtcNow;
    }

    private string GetErrorsText(IdentityResult result)
    {
        return string.Join(", ", result.Errors.Select(e => e.Description));
    }
}