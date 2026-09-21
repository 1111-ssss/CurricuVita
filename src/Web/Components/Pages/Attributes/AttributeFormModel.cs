using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Web.Resources;

namespace Web.Components.Pages.Attributes;

public class AttributeFormModel
{
    [Required(ErrorMessageResourceName = nameof(SharedResource.Validation_Required), ErrorMessageResourceType = typeof(SharedResource))]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessageResourceName = nameof(SharedResource.Validation_Required), ErrorMessageResourceType = typeof(SharedResource))]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public AttributeDataType DataType { get; set; } = AttributeDataType.String;

    public string OptionsText { get; set; } = string.Empty;

    public List<string> GetOptions() => OptionsText
        .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToList();
}
