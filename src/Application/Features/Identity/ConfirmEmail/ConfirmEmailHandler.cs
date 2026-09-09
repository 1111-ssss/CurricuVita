using Domain.Interfaces.Identity;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Identity.ConfirmEmail;

public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailRequest, Result>
{
    private readonly IIdentityService _identityService;

    public ConfirmEmailHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _identityService.ConfirmEmail(request.Id, request.Token);

        return result;
    }
}