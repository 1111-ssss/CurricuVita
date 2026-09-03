using Domain.Entities;
using Domain.Interfaces.Identity;
using Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Domain.ResultPattern.Result;
using Domain.ResultPattern.Errors;
using Ardalis.Specification;

namespace Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IRepositoryBase<User> _userRepository;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IRepositoryBase<User> userRepository
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
    }

    public async Task<Result<int>> ExternalLoginAsync(
        string provider,
        string providerKey,
        string email,
        string? firstName,
        string? lastName,
        string? password,
        CancellationToken cancellationToken = default
    )
    {
        var info = new UserLoginInfo(provider, providerKey, provider);
        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        if (user is not null)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Result<int>.Success(user.Id);
        }

        user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            if (firstName is null || lastName is null || password is null)
            {
                return Result.Failure(Errors.ExternalLoginError);
            }

            return await RegisterUserAsync(email, firstName, lastName, password, cancellationToken);
        }

        var addLoginResult = await _userManager.AddLoginAsync(user, info);
        if (!addLoginResult.Succeeded)
        {
            return Result.Failure(
                Errors.ExternalLoginError with {
                    Message = string.Join(", ", addLoginResult.Errors.Select(e => e.Description))
                }
            );
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return Result<int>.Success(user.Id);
    }

    public async Task<bool> IsEmailConfirmedAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user?.EmailConfirmed ?? false;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return null;
        }

        return await _userRepository.GetByIdAsync(user.Id);
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<Result<int>> RegisterUserAsync(
        string email,
        string firstName,
        string lastName,
        string password,
        CancellationToken cancellationToken = default
    )
    {
        var applicationUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = false
        };

        var passwordHash = _userManager.PasswordHasher.HashPassword(applicationUser, password);
        applicationUser.PasswordHash = passwordHash;

        var createResult = await _userManager.CreateAsync(applicationUser);
        if (!createResult.Succeeded)
        {
            return Result.Failure(
                Errors.ExternalLoginError with {
                    Message = string.Join(", ", createResult.Errors.Select(e => e.Description))
                }
            );
        }

        var domainUser = new User
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Version = 1
        };

        await _userRepository.AddAsync(domainUser);
        await _userRepository.SaveChangesAsync();

        return Result<int>.Success(domainUser.Id);
    }
}