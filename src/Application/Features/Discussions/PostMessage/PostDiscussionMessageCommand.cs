using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Discussions.PostMessage;

public record PostDiscussionMessageCommand(
    int PositionId,
    int AuthorId,
    string ContentMarkdown
) : IRequest<Result<int>>;
