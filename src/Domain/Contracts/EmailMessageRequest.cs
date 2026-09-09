namespace Domain.Contracts;

public record EmailMessageRequest(
    string ToEmail,
    string Subject,
    string Body
);