using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Profile;

public sealed class TagSuggestionsSpec : Specification<Tag, string>
{
    public TagSuggestionsSpec(string? prefix, int take)
    {
        if (!string.IsNullOrWhiteSpace(prefix))
        {
            var normalized = prefix.Trim().ToLower();
            Query.Where(t => t.Name.ToLower().Contains(normalized));
        }

        Query
            .OrderBy(t => t.Name)
            .Take(Math.Clamp(take, 1, 50))
            .Select(t => t.Name);
    }
}
