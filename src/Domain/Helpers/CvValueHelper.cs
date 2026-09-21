using Domain.Entities;
using Domain.Enums;

namespace Domain.Helpers;

public static class CvValueHelper
{
    public static bool IsFilled(AttributeDataType dataType, UserAttributeValue? value)
    {
        if (value is null)
        {
            return false;
        }

        return dataType switch
        {
            AttributeDataType.String => !string.IsNullOrWhiteSpace(value.StringValue),
            AttributeDataType.Text => !string.IsNullOrWhiteSpace(value.TextValue),
            AttributeDataType.Image => !string.IsNullOrWhiteSpace(value.ImageValue),
            AttributeDataType.Numeric => value.NumericValue.HasValue,
            AttributeDataType.Date => value.DateValue.HasValue,
            AttributeDataType.Period => value.PeriodStartValue.HasValue || value.PeriodEndValue.HasValue,
            AttributeDataType.Boolean => value.BooleanValue.HasValue,
            AttributeDataType.Dropdown => !string.IsNullOrWhiteSpace(value.StringValue),
            _ => false
        };
    }

    public static string FormatValue(AttributeDataType dataType, UserAttributeValue? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        return dataType switch
        {
            AttributeDataType.String => value.StringValue ?? string.Empty,
            AttributeDataType.Text => value.TextValue ?? string.Empty,
            AttributeDataType.Image => value.ImageValue ?? string.Empty,
            AttributeDataType.Numeric => value.NumericValue?.ToString() ?? string.Empty,
            AttributeDataType.Date => value.DateValue?.ToString("d") ?? string.Empty,
            AttributeDataType.Period => FormatPeriod(value.PeriodStartValue, value.PeriodEndValue),
            AttributeDataType.Boolean => value.BooleanValue.HasValue ? (value.BooleanValue.Value ? "+" : "-") : string.Empty,
            AttributeDataType.Dropdown => value.StringValue ?? string.Empty,
            _ => string.Empty
        };
    }

    private static string FormatPeriod(DateTime? start, DateTime? end)
    {
        if (start is null && end is null)
        {
            return string.Empty;
        }
        return $"{start?.ToString("d") ?? "…"} — {end?.ToString("d") ?? "…"}";
    }
}
