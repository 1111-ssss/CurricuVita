using Domain.Contracts.SupportContracts;

namespace Application.Features.Integrations.CreateSupportTicket;

public record SupportTicketResult(
    string FileName,
    SupportTicketPayload Payload
);
