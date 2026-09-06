using Domain.Interfaces.Identity;
using MediatR;

namespace Application.Features.Identity.Logout;

public class LogoutHandler : IRequestHandler<LogoutRequest>
{
    private readonly IIdentityService _identityService;

    public LogoutHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
        await _identityService.Logout();
    }
}