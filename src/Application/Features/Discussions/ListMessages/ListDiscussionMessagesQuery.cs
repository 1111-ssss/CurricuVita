using Domain.Contracts.DiscussionContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Discussions.ListMessages;

public record ListDiscussionMessagesQuery(
    int PositionId
) : IRequest<Result<List<DiscussionMessageDto>>>;
