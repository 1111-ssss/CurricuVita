using Domain.Entities;

namespace Domain.Helpers;

public static class CvProjectFilter
{
    public static List<Project> Filter(
        IReadOnlyList<Project> all,
        IReadOnlyList<string> requiredTags,
        int? maxCount
    )
    {
        IEnumerable<Project> query = all;

        if (requiredTags.Count > 0)
        {
            var wanted = new HashSet<string>(requiredTags.Select(t => t.Trim().ToLowerInvariant()).Where(t => t.Length > 0));
            query = query.Where(p => p.Tags.Any(pt =>
                pt.Tag != null && wanted.Contains((pt.Tag.Name ?? string.Empty).Trim().ToLowerInvariant()))
            );
        }

        query = query
            .OrderByDescending(p => (p.EndDate ?? p.StartDate) ?? DateTime.MinValue)
            .ThenByDescending(p => p.Id);

        if (maxCount.HasValue && maxCount.Value > 0)
        {
            query = query.Take(maxCount.Value);
        }

        return query.ToList();
    }
}
