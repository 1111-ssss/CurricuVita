namespace Domain.Contracts.UserContracts;

public record EmailMessageRequest(
    string ToEmail,
    string Subject,
    string Body
);
