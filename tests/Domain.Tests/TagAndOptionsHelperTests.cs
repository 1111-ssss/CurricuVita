using Domain.Constants;
using Domain.Enums;
using Domain.Helpers;
using Xunit;

namespace Domain.Tests;

public class TagAndOptionsHelperTests
{
    [Fact]
    public void NormalizeTags_Null_ReturnsEmpty()
    {
        Assert.Empty(TagHelper.NormalizeTags(null));
    }

    [Fact]
    public void NormalizeTags_DedupesCaseInsensitivelyAndSorts()
    {
        var result = TagHelper.NormalizeTags(["Python", "  python ", "Java", " "]);
        Assert.Equal(["Java", "Python"], result);
    }

    [Fact]
    public void NormalizeTags_CapsLengthAndCount()
    {
        var many = Enumerable.Range(0, 30).Select(i => $"tag{i:00}").ToList();
        many.Add(new string('x', 60));

        var result = TagHelper.NormalizeTags(many);

        Assert.Equal(20, result.Count);
        Assert.DoesNotContain(new string('x', 60), result);
    }

    [Fact]
    public void NormalizeOptions_NonDropdown_ReturnsEmpty()
    {
        Assert.Empty(AttributeOptionsHelper.NormalizeOptions(AttributeDataType.String, ["a"]));
    }

    [Fact]
    public void NormalizeOptions_Dropdown_TrimsAndDedupes()
    {
        var result = AttributeOptionsHelper.NormalizeOptions(
            AttributeDataType.Dropdown, [" Pro ", "pro", "Expert", " "]);
        Assert.Equal(["Pro", "Expert"], result);
    }

    [Fact]
    public void SerializeOptions_Empty_ReturnsNull()
    {
        Assert.Null(AttributeOptionsHelper.SerializeOptions([]));
    }

    [Fact]
    public void DeserializeOptions_InvalidJson_ReturnsEmpty()
    {
        Assert.Empty(AttributeOptionsHelper.DeserializeOptions("not-json"));
    }

    [Fact]
    public void DeserializeOptions_RoundTrip()
    {
        var json = AttributeOptionsHelper.SerializeOptions(["None", "Essentials", "Pro", "Expert"]);
        Assert.Equal(["None", "Essentials", "Pro", "Expert"],
            AttributeOptionsHelper.DeserializeOptions(json));
    }

    [Theory]
    [InlineData("junior", "Junior")]
    [InlineData("C-LEVEL", "C-level")]
    [InlineData("Middle", "Middle")]
    public void PositionLevels_Normalize_Canonicalizes(string input, string expected)
    {
        Assert.Equal(expected, PositionLevels.Normalize(input));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("Intern")]
    public void PositionLevels_Normalize_Unknown_ReturnsNull(string? input)
    {
        Assert.Null(PositionLevels.Normalize(input));
    }
}
