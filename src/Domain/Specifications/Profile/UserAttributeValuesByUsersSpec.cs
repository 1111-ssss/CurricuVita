using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Profile;

public sealed class UserAttributeValuesByUsersSpec : Specification<UserAttributeValue>
{
    public UserAttributeValuesByUsersSpec(IEnumerable<int> userIds, IEnumerable<int> attributeDefinitionIds)
    {
        var users = userIds.Distinct().ToList();
        var attributes = attributeDefinitionIds.Distinct().ToList();

        Query
            .Where(v => users.Contains(v.UserId) && attributes.Contains(v.AttributeDefinitionId))
            .Include(v => v.AttributeDefinition);
    }
}
