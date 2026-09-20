using Ardalis.Specification;
using Domain.Contracts.DiscussionContracts;
using Domain.Entities;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Discussions;
using Domain.Specifications.Positions;
using MediatR;

namespace Application.Features.Discussions.ListMessages;

public class ListDiscussionMessagesHandler : IRequestHandler<ListDiscussionMessagesQuery, Result<List<DiscussionMessageDto>>>
{
    private readonly IPositionRepository _positions;
    private readonly IRepositoryBase<DiscussionMessage> _messages;

    public ListDiscussionMessagesHandler(
        IPositionRepository positions,
        IRepositoryBase<DiscussionMessage> messages
    )
    {
        _positions = positions;
        _messages = messages;
    }

    public async Task<Result<List<DiscussionMessageDto>>> Handle(
        ListDiscussionMessagesQuery request,
        CancellationToken cancellationToken
    )
    {
        if (!await _positions.AnyAsync(new PositionExistsSpec(request.PositionId), cancellationToken))
        {
            return Result<List<DiscussionMessageDto>>.Failure(Errors.PositionNotFound);
        }

        var messages = await _messages.ListAsync(
            new MessagesByPositionSpec(request.PositionId), cancellationToken
        );

        return Result<List<DiscussionMessageDto>>.Success(
            messages.Select(m => new DiscussionMessageDto(
                m.Id,
                m.PositionId,
                m.AuthorId,
                FormatAuthor(m.Author),
                m.CreatedAt,
                m.ContentMarkdown,
                string.Empty
            )).ToList()
        );
    }

    private static string FormatAuthor(User? author)
    {
        if (author is null)
        {
            return "?";
        }

        var name = $"{author.FirstName} {author.LastName}".Trim();
        return string.IsNullOrWhiteSpace(name) ? author.Email ?? $"User {author.Id}" : name;
    }
}
