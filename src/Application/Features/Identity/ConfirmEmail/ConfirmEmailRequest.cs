using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Identity.ConfirmEmail;

public record ConfirmEmailRequest(
    int Id,
    string Token
) : IRequest<Result>;