using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Discussions;

public sealed class MessagesByPositionSpec : Specification<DiscussionMessage>
{
    public MessagesByPositionSpec(int positionId, int take = 200)
    {
        Query
            .Where(m => m.PositionId == positionId)
            .Include(m => m.Author)
            .OrderBy(m => m.CreatedAt)
            .ThenBy(m => m.Id)
            .Take(take);
    }
}
