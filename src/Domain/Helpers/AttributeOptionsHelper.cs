using System.Text.Json;
using Domain.Enums;

namespace Domain.Helpers;

public static class AttributeOptionsHelper
{
    public const string ProtectedCategory = "Me";

    public static List<string> NormalizeOptions(AttributeDataType dataType, List<string>? options)
    {
        if (dataType != AttributeDataType.Dropdown || options is null)
        {
            return new List<string>();
        }

        return options
            .Where(o => !string.IsNullOrWhiteSpace(o))
            .Select(o => o.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public static string? SerializeOptions(List<string> options)
        => options.Count == 0 ? null : JsonSerializer.Serialize(options);

    public static List<string> DeserializeOptions(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<string>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
        catch (JsonException)
        {
            return new List<string>();
        }
    }
}
