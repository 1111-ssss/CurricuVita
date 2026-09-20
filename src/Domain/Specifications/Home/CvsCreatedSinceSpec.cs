using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Home;

public sealed class CvsCreatedSinceSpec : Specification<CV>
{
    public CvsCreatedSinceSpec(DateTime since)
    {
        Query.Where(c => c.CreatedAt >= since);
    }
}
