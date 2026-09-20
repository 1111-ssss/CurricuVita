namespace Domain.Constants;

public static class PositionLevels
{
    public static readonly IReadOnlyList<string> All = ["Junior", "Middle", "Senior", "C-level"];

    public static string? Normalize(string? level)
    {
        if (string.IsNullOrWhiteSpace(level))
        {
            return null;
        }

        var trimmed = level.Trim();
        return All.FirstOrDefault(a => string.Equals(a, trimmed, StringComparison.OrdinalIgnoreCase));
    }
}
