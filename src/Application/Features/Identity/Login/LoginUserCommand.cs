using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Identity.Login;

public record LoginUserCommand(
    string Email,
    string Password
) : IRequest<Result>;