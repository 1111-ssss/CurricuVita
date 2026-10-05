using Domain.Contracts.PositionContracts;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Helpers;

public static class PositionAggregation
{
    public static List<PositionNumericAggregateDto> Numeric(
        IEnumerable<UserAttributeValue> values,
        IReadOnlyDictionary<int, string> names
    )
    {
        return values
            .Where(v => v.NumericValue.HasValue)
            .GroupBy(v => v.AttributeDefinitionId)
            .Select(g =>
            {
                var nums = g.Select(v => v.NumericValue!.Value).ToList();
                var name = names.TryGetValue(g.Key, out var n) ? n : $"#{g.Key}";
                return new PositionNumericAggregateDto(
                    g.Key,
                    name,
                    nums.Count,
                    (double)nums.Average(v => v),
                    nums.Min(),
                    nums.Max());
            })
            .OrderBy(a => a.Name)
            .ToList();
    }

    public static List<PositionTextAggregateDto> TextTop(
        IEnumerable<UserAttributeValue> values,
        IReadOnlyDictionary<int, (string Name, AttributeDataType DataType)> definitions,
        int take = 3
    )
    {
        return values
            .Where(v => definitions.TryGetValue(v.AttributeDefinitionId, out var d)
                && (d.DataType == AttributeDataType.String
                    || d.DataType == AttributeDataType.Text
                    || d.DataType == AttributeDataType.Dropdown)
                && !string.IsNullOrWhiteSpace(TextKey(v, d.DataType)))
            .GroupBy(v => v.AttributeDefinitionId)
            .Select(g =>
            {
                var top = g
                    .GroupBy(v => TextKey(v, definitions[g.Key].DataType)!.Trim())
                    .Select(t => new PositionTextValueDto(t.Key, t.Count()))
                    .OrderByDescending(t => t.Count)
                    .ThenBy(t => t.Value, StringComparer.OrdinalIgnoreCase)
                    .Take(Math.Max(take, 1))
                    .ToList();
                return new PositionTextAggregateDto(
                    g.Key,
                    definitions[g.Key].Name,
                    g.Count(),
                    top);
            })
            .OrderBy(a => a.Name)
            .ToList();
    }

    public static string? TextKey(
        UserAttributeValue value,
        AttributeDataType dataType
    )
    {
        return dataType switch
        {
            AttributeDataType.Text => value.TextValue,
            _ => value.StringValue
        };
    }
}
