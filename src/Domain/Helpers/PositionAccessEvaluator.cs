using Domain.Entities;
using Domain.Enums;

namespace Domain.Helpers;

public static class PositionAccessEvaluator
{
    public static bool HasAccess(Position position, IReadOnlyList<UserAttributeValue> values)
    {
        if (position.IsPublic)
        {
            return true;
        }

        if (position.AccessRules.Count == 0)
        {
            return false;
        }

        var byAttribute = values
            .GroupBy(v => v.AttributeDefinitionId)
            .ToDictionary(g => g.Key, g => g.First());

        foreach (var rule in position.AccessRules)
        {
            if (!byAttribute.TryGetValue(rule.AttributeDefinitionId, out var value))
            {
                return false;
            }

            var dataType = rule.AttributeDefinition?.DataType ?? AttributeDataType.String;
            if (!MatchesRule(dataType, value, rule.Operator, rule.Value))
            {
                return false;
            }
        }

        return true;
    }

    public static bool MatchesRule(AttributeDataType dataType, UserAttributeValue value, Operator op, string expected)
    {
        expected = (expected ?? string.Empty).Trim();

        switch (dataType)
        {
            case AttributeDataType.Numeric:
                if (!decimal.TryParse(expected, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var expectedNum) || !value.NumericValue.HasValue)
                {
                    return false;
                }
                return Compare(value.NumericValue.Value.CompareTo(expectedNum), op);

            case AttributeDataType.Date:
                if (!DateTime.TryParse(expected, out var expectedDate) || !value.DateValue.HasValue)
                {
                    return false;
                }
                return Compare(value.DateValue.Value.Date.CompareTo(expectedDate.Date), op);

            case AttributeDataType.Boolean:
                if (!TryParseBool(expected, out var expectedBool) || !value.BooleanValue.HasValue)
                {
                    return false;
                }
                return op switch
                {
                    Operator.Equal => value.BooleanValue.Value == expectedBool,
                    Operator.NotEqual => value.BooleanValue.Value != expectedBool,
                    _ => false
                };

            case AttributeDataType.Period:
                var actual = value.PeriodStartValue ?? value.PeriodEndValue;
                if (!DateTime.TryParse(expected, out var expectedPeriod) || actual is null)
                {
                    return false;
                }
                return Compare(actual.Value.Date.CompareTo(expectedPeriod.Date), op);

            default:
                var actualStr = GetStringValue(dataType, value) ?? string.Empty;
                return op switch
                {
                    Operator.Equal => string.Equals(actualStr.Trim(), expected, StringComparison.OrdinalIgnoreCase),
                    Operator.NotEqual => !string.Equals(actualStr.Trim(), expected, StringComparison.OrdinalIgnoreCase),
                    Operator.Contains => actualStr.Contains(expected, StringComparison.OrdinalIgnoreCase),
                    Operator.NotContains => !actualStr.Contains(expected, StringComparison.OrdinalIgnoreCase),
                    Operator.GreaterThan => string.Compare(actualStr, expected, StringComparison.OrdinalIgnoreCase) > 0,
                    Operator.LessThan => string.Compare(actualStr, expected, StringComparison.OrdinalIgnoreCase) < 0,
                    Operator.GreaterThanOrEqual => string.Compare(actualStr, expected, StringComparison.OrdinalIgnoreCase) >= 0,
                    Operator.LessThanOrEqual => string.Compare(actualStr, expected, StringComparison.OrdinalIgnoreCase) <= 0,
                    _ => false
                };
        }
    }

    private static string? GetStringValue(AttributeDataType dataType, UserAttributeValue value) => dataType switch
    {
        AttributeDataType.String => value.StringValue,
        AttributeDataType.Text => value.TextValue,
        AttributeDataType.Image => value.ImageValue,
        AttributeDataType.Dropdown => value.StringValue,
        AttributeDataType.Date => value.DateValue?.ToString("o"),
        AttributeDataType.Numeric => value.NumericValue?.ToString(System.Globalization.CultureInfo.InvariantCulture),
        AttributeDataType.Boolean => value.BooleanValue?.ToString(),
        AttributeDataType.Period => (value.PeriodStartValue ?? value.PeriodEndValue)?.ToString("o"),
        _ => null
    };

    private static bool Compare(int cmp, Operator op) => op switch
    {
        Operator.Equal => cmp == 0,
        Operator.NotEqual => cmp != 0,
        Operator.GreaterThan => cmp > 0,
        Operator.LessThan => cmp < 0,
        Operator.GreaterThanOrEqual => cmp >= 0,
        Operator.LessThanOrEqual => cmp <= 0,
        Operator.Contains => cmp == 0,
        Operator.NotContains => cmp != 0,
        _ => false
    };

    private static bool TryParseBool(string s, out bool result)
    {
        if (bool.TryParse(s, out result))
        {
            return true;
        }
        if (s == "1")
        {
            result = true;
            return true;
        }
        if (s == "0")
        {
            result = false;
            return true;
        }
        if (string.Equals(s, "checked", StringComparison.OrdinalIgnoreCase) || string.Equals(s, "yes", StringComparison.OrdinalIgnoreCase))
        {
            result = true;
            return true;
        }
        if (string.Equals(s, "unchecked", StringComparison.OrdinalIgnoreCase) || string.Equals(s, "no", StringComparison.OrdinalIgnoreCase))
        {
            result = false;
            return true;
        }
        return false;
    }
}
