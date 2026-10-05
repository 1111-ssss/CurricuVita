using Domain.Enums;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Integrations.CreateSupportTicket;

public record CreateSupportTicketCommand(
    int UserId,
    int? PositionId,
    string Link,
    string Summary,
    SupportTicketPriority Priority
) : IRequest<Result<SupportTicketResult>>;
