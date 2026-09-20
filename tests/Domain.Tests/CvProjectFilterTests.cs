using Domain.Entities;
using Domain.Helpers;
using Xunit;

namespace Domain.Tests;

public class CvProjectFilterTests
{
    [Fact]
    public void Filter_NoTagsNoLimit_ReturnsAllMostRecentFirst()
    {
        var all = new List<Project>
        {
            MakeProject(1, new DateTime(2021, 1, 1)),
            MakeProject(2, new DateTime(2023, 6, 1)),
            MakeProject(3, new DateTime(2022, 1, 1))
        };

        var result = CvProjectFilter.Filter(all, [], null);

        Assert.Equal([2, 3, 1], result.Select(p => p.Id).ToList());
    }

    [Fact]
    public void Filter_ByTags_MatchesCaseInsensitively()
    {
        var all = new List<Project>
        {
            MakeProject(1, new DateTime(2023, 1, 1), "Python"),
            MakeProject(2, new DateTime(2023, 2, 1), "python", "Data Engineering"),
            MakeProject(3, new DateTime(2023, 3, 1), "Java")
        };

        var result = CvProjectFilter.Filter(all, ["PYTHON"], null);

        Assert.Equal([2, 1], result.Select(p => p.Id).ToList());
    }

    [Fact]
    public void Filter_MaxCount_TakesMostRecentMatching()
    {
        var all = new List<Project>
        {
            MakeProject(1, new DateTime(2021, 1, 1), "Python"),
            MakeProject(2, new DateTime(2022, 1, 1), "Python"),
            MakeProject(3, new DateTime(2023, 1, 1), "Python"),
            MakeProject(4, new DateTime(2024, 1, 1), "Java")
        };

        var result = CvProjectFilter.Filter(all, ["Python"], 2);

        Assert.Equal([3, 2], result.Select(p => p.Id).ToList());
    }

    [Fact]
    public void Filter_NonPositiveMaxCount_IgnoresLimit()
    {
        var all = new List<Project> { MakeProject(1, new DateTime(2021, 1, 1)) };

        Assert.Single(CvProjectFilter.Filter(all, [], 0));
    }

    private static Project MakeProject(int id, DateTime? end, params string[] tags)
    {
        var p = new Project
        {
            Id = id,
            Title = $"P{id}",
            StartDate = new DateTime(2020, 1, 1),
            EndDate = end
        };
        foreach (var t in tags)
        {
            p.Tags.Add(new ProjectTag { Tag = new Tag { Name = t } });
        }
        return p;
    }
}
