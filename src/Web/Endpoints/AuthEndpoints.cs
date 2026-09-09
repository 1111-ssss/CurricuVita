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
        group.MapPost("logout", Logout);
        group.MapPost("register", Register);
        group.MapGet("confirm-email", ConfirmEmail);

        return endpoints;
    }

    private static async Task<IResult> Login(
        [FromServices] IMediator mediator,
        LoginUserRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var result = await mediator.Send(request, cancellationToken);

        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> Logout(
        [FromServices] IMediator mediator,
        LogoutRequest request,
        CancellationToken cancellationToken = default
    )
    {
        await mediator.Send(request, cancellationToken);

        return Results.Unauthorized();
    }

    private static async Task<IResult> Register(
        [FromServices] IMediator mediator,
        RegisterUserRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var result = await mediator.Send(request, cancellationToken);

        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> ConfirmEmail(
        [FromServices] IMediator mediator,
        [AsParameters] ConfirmEmailRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var result = await mediator.Send(request, cancellationToken);

        return result.ToMinimalApiResult();
    }
}