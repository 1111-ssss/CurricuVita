using Domain.Contracts;

namespace Infrastructure.Interfaces;

public interface IEmailQueueService
{
    ValueTask EnqueueEmail(
        EmailMessageRequest request,
        CancellationToken cancellationToken = default
    );
    ValueTask<EmailMessageRequest> DequeueEmail(
        CancellationToken cancellationToken = default
    );
}