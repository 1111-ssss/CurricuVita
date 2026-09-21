using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Contracts.PositionContracts;
using Web.Resources;

namespace Web.Components.Pages.Positions;

public class PositionFormModel
{
    [Required(ErrorMessageResourceName = nameof(SharedResource.Validation_Required), ErrorMessageResourceType = typeof(SharedResource))]
    [MaxLength(250)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string DescriptionMarkdown { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Company { get; set; }

    public string? Level { get; set; }

    public bool IsPublic { get; set; } = true;

    [Range(1, 50)]
    public int? MaxProjectCount { get; set; }

    public HashSet<int> SelectedAttributeIds { get; set; } = new();

    public HashSet<int> RequiredAttributeIds { get; set; } = new();

    public List<AccessRuleRow> AccessRules { get; set; } = new();

    public string TagsText { get; set; } = string.Empty;

    public List<string> GetTags() => TagsText
        .Split([',', ';', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Where(t => t.Length is > 0 and <= 50)
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .Take(20)
        .ToList();

    public List<PositionAttributeInput> ToAttributeInputs() =>
        SelectedAttributeIds
            .Select(id => new PositionAttributeInput(
                id,
                RequiredAttributeIds.Contains(id)))
            .ToList();

    public List<PositionAccessRuleInput> ToRuleInputs() =>
        AccessRules
            .Where(r => r.AttributeDefinitionId > 0 && !string.IsNullOrWhiteSpace(r.Value))
            .Select(r => new PositionAccessRuleInput(
                r.AttributeDefinitionId,
                r.Operator,
                r.Value.Trim()))
            .ToList();
}

public class AccessRuleRow
{
    public int AttributeDefinitionId { get; set; }
    public Operator Operator { get; set; } = Operator.Equal;
    public string Value { get; set; } = string.Empty;
}
