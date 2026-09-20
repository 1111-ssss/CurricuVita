using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Profile;

public sealed class UserAttributeValuesByUserSpec : Specification<UserAttributeValue>
{
    public UserAttributeValuesByUserSpec(int userId)
    {
        Query
            .Where(v => v.UserId == userId)
            .Include(v => v.AttributeDefinition)
            .OrderBy(v => v.AttributeDefinition.Category)
            .ThenBy(v => v.AttributeDefinition.Name);
    }
}
