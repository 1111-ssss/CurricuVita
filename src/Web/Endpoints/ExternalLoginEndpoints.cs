using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain.Interfaces.Identity;
using System.Security.Claims;
using Domain.Entities;

namespace Web.Endpoints;

public static class ExternalLoginEndpoints
{
    public static IEndpointRouteBuilder MapExternalLoginEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/external-login");

        group.MapGet("", ExternalLogin);
        group.MapGet("callback", ExternalLoginCallback);

        return endpoints;
    }

    private static IResult ExternalLogin(
        [FromQuery] string provider,
        [FromQuery] string? returnUrl,
        [FromServices] SignInManager<User> signInManager)
    {
        returnUrl ??= "/";
        if (!Uri.IsWellFormedUriString(returnUrl, UriKind.Relative) &&
            !returnUrl.StartsWith("/"))
        {
            returnUrl = "/";
        }

        var redirectUrl = $"/api/external-login/callback?returnUrl={Uri.EscapeDataString(returnUrl)}";

        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Results.Challenge(properties, new[] { provider });
    }

    private static async Task<IResult> ExternalLoginCallback(
        [FromQuery] string? returnUrl,
        [FromServices] SignInManager<User> signInManager,
        [FromServices] IIdentityService identityService,
        [FromServices] IConfiguration configuration,
        CancellationToken cancellationToken)
    {
        returnUrl ??= "/";

        var info = await signInManager.GetExternalLoginInfoAsync();
        var loginPath = configuration.GetValue<string>("Identity:Cookie:LoginPath") ?? "/login";

        if (info is null)
        {
            return Results.Redirect($"{loginPath}?error=external_login_failed");
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return Results.Redirect($"{loginPath}?error=email_required");
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
            var error = result.Error?.Message ?? "unknown";
            return Results.Redirect($"{loginPath}?error={Uri.EscapeDataString(error)}");
        }

        return Results.Redirect(returnUrl);
    }
}