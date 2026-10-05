using Domain.Constants;
using Domain.Contracts.SupportContracts;
using Domain.Interfaces.Database;
using Domain.Interfaces.Identity;
using Domain.Interfaces.Services;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Positions;
using MediatR;

namespace Application.Features.Integrations.CreateSupportTicket;

public class CreateSupportTicketHandler : IRequestHandler<CreateSupportTicketCommand, Result<SupportTicketResult>>
{
    private readonly IUserRepository _users;
    private readonly IIdentityService _identity;
    private readonly IPositionRepository _positions;
    private readonly IEnumerable<ISupportTicketSink> _sinks;

    public CreateSupportTicketHandler(
        IUserRepository users,
        IIdentityService identity,
        IPositionRepository positions,
        IEnumerable<ISupportTicketSink> sinks
    )
    {
        _users = users;
        _identity = identity;
        _positions = positions;
        _sinks = sinks;
    }

    public async Task<Result<SupportTicketResult>> Handle(
        CreateSupportTicketCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _users.GetByIdAsync(
            request.UserId,
            cancellationToken
        );
        if (user is null)
        {
            return Result<SupportTicketResult>.Failure(Errors.UserNotFound);
        }

        string? positionTitle = null;
        if (request.PositionId.HasValue)
        {
            var position = await _positions.SingleOrDefaultAsync(
                new PositionByIdSpec(request.PositionId.Value),
                cancellationToken
            );
            if (position is null)
            {
                return Result<SupportTicketResult>.Failure(Errors.PositionNotFound);
            }
            positionTitle = position.Title;
        }

        var roles = (await _identity.GetRoles(request.UserId)).OrderBy(r => r).ToList();
        var adminEmails = await ResolveAdminEmails(
            cancellationToken
        );

        var payload = new SupportTicketPayload(
            $"{user.Email ?? user.UserName} ({string.Join(", ", roles)})",
            roles,
            positionTitle,
            request.Link.Trim(),
            request.Priority.ToString(),
            request.Summary.Trim(),
            adminEmails,
            DateTime.UtcNow
        );

        var fileName = BuildFileName();
        foreach (var sink in _sinks)
        {
            var stored = await sink.Store(
                fileName,
                payload,
                cancellationToken
            );
            if (!stored.IsSuccess)
            {
                return Result<SupportTicketResult>.Failure(stored.Error!);
            }
        }

        return Result<SupportTicketResult>.Success(new SupportTicketResult(fileName, payload));
    }

    private async Task<List<string>> ResolveAdminEmails(
        CancellationToken cancellationToken
    )
    {
        var allUsers = await _identity.ListUsersAsync(
            null,
            200,
            0,
            cancellationToken
        );
        if (!allUsers.IsSuccess)
        {
            return new List<string>();
        }
        return allUsers.Value
            .Where(u => u.Roles.Contains(UserRoles.Administrator))
            .Select(u => u.Email)
            .ToList();
    }

    private static string BuildFileName()
    {
        var stamp = DateTime.UtcNow.ToString(SupportTicketConstants.TimestampFormat);
        var name = SupportTicketConstants.FilePrefix + stamp + "-" + Guid.NewGuid().ToString("N");
        return name[..SupportTicketConstants.FileNameLength] + SupportTicketConstants.FileExtension;
    }
}
