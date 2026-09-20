using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Identity.ConfirmEmail;

public record ConfirmEmailCommand(
    int Id,
    string Token
) : IRequest<Result>;