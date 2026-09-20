using Ardalis.Specification;
using Domain.Entities;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Positions;
using MediatR;

namespace Application.Features.Discussions.PostMessage;

public class PostDiscussionMessageHandler : IRequestHandler<PostDiscussionMessageCommand, Result<int>>
{
    private readonly IPositionRepository _positions;
    private readonly IRepositoryBase<DiscussionMessage> _messages;

    public PostDiscussionMessageHandler(
        IPositionRepository positions,
        IRepositoryBase<DiscussionMessage> messages
    )
    {
        _positions = positions;
        _messages = messages;
    }

    public async Task<Result<int>> Handle(PostDiscussionMessageCommand request, CancellationToken cancellationToken)
    {
        if (!await _positions.AnyAsync(new PositionExistsSpec(request.PositionId), cancellationToken))
        {
            return Result<int>.Failure(Errors.PositionNotFound);
        }

        var text = request.ContentMarkdown?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(text) || text.Length > 5000)
        {
            return Result<int>.Failure(Errors.DiscussionInvalidContent);
        }

        var message = new DiscussionMessage
        {
            PositionId = request.PositionId,
            AuthorId = request.AuthorId,
            ContentMarkdown = text,
            CreatedAt = DateTime.UtcNow
        };

        await _messages.AddAsync(message, cancellationToken);
        await _messages.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(message.Id);
    }
}
