using MediatR;
using Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Identity.Login;
using Application.Features.Identity.Logout;
using Application.Features.Identity.Register;
using Application.Features.Identity.ConfirmEmail;

namespace Web.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth");

        group.MapPost("login", Login);
        group.MapPost("login-form", LoginForm).DisableAntiforgery();
        group.MapPost("logout", Logout);
        group.MapPost("logout-form", LogoutForm).DisableAntiforgery();
        group.MapPost("register", Register);
        group.MapGet("confirm-email", ConfirmEmail);

        return endpoints;
    }

    private static async Task<IResult> Login(
        [FromServices] IMediator mediator,
        LoginUserCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var result = await mediator.Send(request, cancellationToken);

        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> LoginForm(
        [FromServices] IMediator mediator,
        [FromForm] string? email,
        [FromForm] string? password,
        [FromForm] string? returnUrl,
        CancellationToken cancellationToken = default
    )
    {
        var result = await mediator.Send(
            new LoginUserCommand((email ?? string.Empty).Trim(), password ?? string.Empty),
            cancellationToken
        );

        if (result.IsSuccess)
        {
            return Results.Redirect(AuthRedirectHelper.ToLocalUrl(returnUrl));
        }

        return Results.Redirect(
            AuthRedirectHelper.BuildLoginRedirect(result.Error?.Code ?? "LoginFailed", returnUrl)
        );
    }

    private static async Task<IResult> Logout(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default
    )
    {
        await mediator.Send(new LogoutCommand(), cancellationToken);

        return Results.Unauthorized();
    }

    private static async Task<IResult> LogoutForm(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default
    )
    {
        await mediator.Send(new LogoutCommand(), cancellationToken);

        return Results.Redirect(AuthRedirectHelper.HomePagePath);
    }

    private static async Task<IResult> Register(
        [FromServices] IMediator mediator,
        RegisterUserCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var result = await mediator.Send(request, cancellationToken);

        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> ConfirmEmail(
        [FromServices] IMediator mediator,
        [AsParameters] ConfirmEmailCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var result = await mediator.Send(request, cancellationToken);

        return result.ToMinimalApiResult();
    }
}