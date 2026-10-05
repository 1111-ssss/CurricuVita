using Domain.Contracts.SupportContracts;
using Domain.ResultPattern.Result;

namespace Domain.Interfaces.Services;

public interface ISupportTicketSink
{
    Task<Result> Store(
        string fileName,
        SupportTicketPayload payload,
        CancellationToken cancellationToken = default
    );
}
