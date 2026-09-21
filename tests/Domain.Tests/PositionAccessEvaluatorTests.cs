using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Xunit;

namespace Domain.Tests;

public class PositionAccessEvaluatorTests
{
    [Fact]
    public void HasAccess_PublicPosition_AlwaysTrue()
    {
        var p = new Position { Title = "T", IsPublic = true };
        Assert.True(PositionAccessEvaluator.HasAccess(p, []));
    }

    [Fact]
    public void HasAccess_PrivateWithoutRules_ReturnsFalse()
    {
        var p = new Position { Title = "T", IsPublic = false };
        Assert.False(PositionAccessEvaluator.HasAccess(p, []));
    }

    [Fact]
    public void HasAccess_NumericGreaterThan_FiltersCorrectly()
    {
        var p = PositionWithRule(AttributeDataType.Numeric, Operator.GreaterThan, "7.0");
        var ok = new List<UserAttributeValue>
        {
            new() { AttributeDefinitionId = 7, NumericValue = 7.5m }
        };
        var bad = new List<UserAttributeValue>
        {
            new() { AttributeDefinitionId = 7, NumericValue = 6.5m }
        };

        Assert.True(PositionAccessEvaluator.HasAccess(p, ok));
        Assert.False(PositionAccessEvaluator.HasAccess(p, bad));
    }

    [Fact]
    public void HasAccess_MissingValue_ReturnsFalse()
    {
        var p = PositionWithRule(AttributeDataType.Numeric, Operator.GreaterThan, "7.0");
        Assert.False(PositionAccessEvaluator.HasAccess(p, []));
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("checked", true)]
    [InlineData("1", true)]
    public void MatchesRule_BooleanAcceptsCheckedForms(string expected, bool actual)
    {
        var value = new UserAttributeValue { BooleanValue = actual };
        Assert.True(PositionAccessEvaluator.MatchesRule(AttributeDataType.Boolean, value, Operator.Equal, expected));
    }

    [Fact]
    public void MatchesRule_StringContains_IsCaseInsensitive()
    {
        var value = new UserAttributeValue { StringValue = "Advanced Presentation" };
        Assert.True(PositionAccessEvaluator.MatchesRule(
            AttributeDataType.String, value, Operator.Contains, "presentation"));
    }

    private static AttributeDefinition Def(AttributeDataType type) =>
        new() { Name = "X", DataType = type, Category = "Test" };

    private static Position PositionWithRule(AttributeDataType type, Operator op, string value, int attrId = 7)
    {
        var p = new Position { Title = "T", IsPublic = false };
        p.AccessRules.Add(new PositionAccessRule
        {
            AttributeDefinitionId = attrId,
            AttributeDefinition = Def(type),
            Operator = op,
            Value = value
        });
        return p;
    }
}
