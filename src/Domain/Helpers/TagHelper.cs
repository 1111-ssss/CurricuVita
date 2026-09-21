namespace Domain.Helpers;

public static class TagHelper
{
    public static List<string> NormalizeTags(IEnumerable<string>? tags)
    {
        if (tags is null)
        {
            return new List<string>();
        }

        return tags
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t.Trim())
            .Where(t => t.Length > 0 && t.Length <= 50)
            .GroupBy(t => t.ToLowerInvariant())
            .Select(g => g.First())
            .OrderBy(t => t)
            .Take(20)
            .ToList();
    }
}
