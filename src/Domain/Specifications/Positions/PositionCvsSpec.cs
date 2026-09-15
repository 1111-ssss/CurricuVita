using Ardalis.Specification;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Specifications.Positions;

public sealed class PositionCvsSpec : Specification<CV>
{
    public PositionCvsSpec(int positionId, string? searchText = null)
    {
        Query.Where(c => c.PositionId == positionId && c.Status == CvStatus.Published);

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var q = searchText.Trim().ToLower();
            Query.Where(c =>
                c.User.FirstName.ToLower().Contains(q) ||
                c.User.LastName.ToLower().Contains(q) ||
                (c.User.Email != null && c.User.Email.ToLower().Contains(q)));
        }

        Query
            .Include(c => c.User)
            .Include(c => c.Likes)
            .OrderByDescending(c => c.UpdatedAt);
    }
}
