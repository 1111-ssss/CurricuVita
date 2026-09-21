using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Cvs;

public sealed class CvByIdSpec : SingleResultSpecification<CV>
{
    public CvByIdSpec(int id)
    {
        Query
            .Where(c => c.Id == id)
            .Include(c => c.User)
            .Include(c => c.Position)
                .ThenInclude(p => p.RequiredAttributes)
                .ThenInclude(pa => pa.AttributeDefinition)
            .Include(c => c.Position)
                .ThenInclude(p => p.AccessRules)
                .ThenInclude(r => r.AttributeDefinition)
            .Include(c => c.Position)
                .ThenInclude(p => p.RequiredTags)
                .ThenInclude(rt => rt.Tag)
            .Include(c => c.Likes);
    }
}
