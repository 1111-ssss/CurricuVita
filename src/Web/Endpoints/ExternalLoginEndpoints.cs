using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain.Interfaces.Identity;
using Domain.ResultPattern.Errors;
using System.Security.Claims;
using Domain.Entities;

namespace Web.Endpoints;

public static class ExternalLoginEndpoints
{
    public static readonly string[] SupportedProviders = ["Google", "GitHub"];

    public static IEndpointRouteBuilder MapExternalLoginEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/external-login");

        group.MapGet("", ExternalLogin);
        group.MapGet("callback", ExternalLoginCallback);

        return endpoints;
    }

    private static IResult ExternalLogin(
        [FromQuery] string? provider,
        [FromQuery] string? returnUrl,
        [FromServices] SignInManager<User> signInManager)
    {
        if (string.IsNullOrWhiteSpace(provider)
            || !SupportedProviders.Contains(provider, StringComparer.OrdinalIgnoreCase))
        {
            return Results.Redirect(AuthRedirectHelper.BuildLoginRedirect(Errors.ExternalLoginError.Code, returnUrl));
        }

        var normalizedProvider = SupportedProviders.First(p => p.Equals(provider, StringComparison.OrdinalIgnoreCase));
        var safeReturnUrl = AuthRedirectHelper.ToLocalUrl(returnUrl);
        var redirectUrl = $"/api/external-login/callback?returnUrl={Uri.EscapeDataString(safeReturnUrl)}";

        var properties = signInManager.ConfigureExternalAuthenticationProperties(normalizedProvider, redirectUrl);
        return Results.Challenge(properties, new[] { normalizedProvider });
    }

    private static async Task<IResult> ExternalLoginCallback(
        [FromQuery] string? returnUrl,
        [FromServices] SignInManager<User> signInManager,
        [FromServices] IIdentityService identityService,
        CancellationToken cancellationToken)
    {
        var info = await signInManager.GetExternalLoginInfoAsync();

        if (info is null)
        {
            return Results.Redirect(AuthRedirectHelper.BuildLoginRedirect(Errors.ExternalLoginError.Code, returnUrl));
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return Results.Redirect(AuthRedirectHelper.BuildLoginRedirect(Errors.EmailRequired.Code, returnUrl));
        }

        var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)
            ?? info.Principal.FindFirstValue("name")?.Split(' ').FirstOrDefault();

        var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname)
            ?? info.Principal.FindFirstValue("name")?.Split(' ').Skip(1).FirstOrDefault();

        var result = await identityService.ExternalLogin(
            provider: info.LoginProvider,
            providerKey: info.ProviderKey,
            email: email,
            firstName: firstName,
            lastName: lastName,
            cancellationToken: cancellationToken);

        if (!result.IsSuccess)
        {
            var errorCode = result.Error?.Code ?? Errors.ExternalLoginError.Code;
            return Results.Redirect(AuthRedirectHelper.BuildLoginRedirect(errorCode, returnUrl));
        }

        return Results.Redirect(AuthRedirectHelper.ToLocalUrl(returnUrl));
    }
}