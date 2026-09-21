using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Xunit;

namespace Domain.Tests;

public class CvValueHelperTests
{
    [Fact]
    public void IsFilled_NullValue_ReturnsFalse()
    {
        Assert.False(CvValueHelper.IsFilled(AttributeDataType.String, null));
    }

    [Theory]
    [InlineData(AttributeDataType.String)]
    [InlineData(AttributeDataType.Text)]
    [InlineData(AttributeDataType.Image)]
    [InlineData(AttributeDataType.Dropdown)]
    public void IsFilled_BlankStrings_ReturnsFalse(AttributeDataType type)
    {
        var value = new UserAttributeValue { StringValue = "  ", TextValue = " ", ImageValue = "" };
        Assert.False(CvValueHelper.IsFilled(type, value));
    }

    [Fact]
    public void IsFilled_StringSet_ReturnsTrue()
    {
        var value = new UserAttributeValue { StringValue = "Mary" };
        Assert.True(CvValueHelper.IsFilled(AttributeDataType.String, value));
    }

    [Fact]
    public void IsFilled_NumericRequiresValue()
    {
        Assert.False(CvValueHelper.IsFilled(AttributeDataType.Numeric, new UserAttributeValue()));
        Assert.True(CvValueHelper.IsFilled(AttributeDataType.Numeric, new UserAttributeValue { NumericValue = 21 }));
    }

    [Fact]
    public void IsFilled_PeriodRequiresAtLeastOneBound()
    {
        Assert.False(CvValueHelper.IsFilled(AttributeDataType.Period, new UserAttributeValue()));
        Assert.True(CvValueHelper.IsFilled(AttributeDataType.Period,
            new UserAttributeValue { PeriodStartValue = new DateTime(2020, 1, 1) }));
    }

    [Fact]
    public void IsFilled_BooleanRequiresExplicitValue()
    {
        Assert.False(CvValueHelper.IsFilled(AttributeDataType.Boolean, new UserAttributeValue()));
        Assert.True(CvValueHelper.IsFilled(AttributeDataType.Boolean,
            new UserAttributeValue { BooleanValue = false }));
    }

    [Fact]
    public void FormatValue_Numeric_ReturnsInvariantString()
    {
        var value = new UserAttributeValue { NumericValue = 21 };
        Assert.Equal("21", CvValueHelper.FormatValue(AttributeDataType.Numeric, value));
    }

    [Fact]
    public void FormatValue_Null_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, CvValueHelper.FormatValue(AttributeDataType.String, null));
    }
}
