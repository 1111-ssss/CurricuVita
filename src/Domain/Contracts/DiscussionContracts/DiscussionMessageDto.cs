namespace Domain.Contracts.DiscussionContracts;

public record DiscussionMessageDto(
    int Id,
    int PositionId,
    int AuthorId,
    string AuthorName,
    DateTime CreatedAt,
    string ContentMarkdown,
    string ContentHtml
);
