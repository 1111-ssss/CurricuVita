using FluentValidation;

namespace Application.Features.Positions.UpdatePosition;

public class UpdatePositionCommandValidator : AbstractValidator<UpdatePositionCommand>
{
    public UpdatePositionCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Version).GreaterThan(0);

        RuleFor(x => x.Title)
            .Must(t => !string.IsNullOrWhiteSpace(t))
            .WithErrorCode("PositionInvalidTitle");
        RuleFor(x => x.Title)
            .MaximumLength(250)
            .WithErrorCode("PositionInvalidTitle");

        RuleFor(x => x.DescriptionMarkdown)
            .MaximumLength(5000)
            .WithErrorCode("PositionInvalidDescription");

        RuleFor(x => x.MaxProjectCount)
            .Must(v => v is null || (v >= 1 && v <= 50))
            .WithErrorCode("PositionInvalidMaxProjects");

        RuleFor(x => x.Attributes)
            .Must(attrs => attrs == null || attrs
                .Select(a => a.AttributeDefinitionId)
                .Distinct()
                .Count() == attrs.Count)
            .WithErrorCode("PositionAttributeDuplicate");

        RuleForEach(x => x.Attributes)
            .Must(a => a.AttributeDefinitionId > 0)
            .WithErrorCode("PositionAttributeNotFound");

        RuleForEach(x => x.AccessRules)
            .Must(r => r.AttributeDefinitionId > 0)
            .WithErrorCode("PositionAccessRuleInvalid");
        RuleForEach(x => x.AccessRules)
            .Must(r => !string.IsNullOrWhiteSpace(r.Value) && r.Value.Trim().Length <= 200)
            .WithErrorCode("PositionAccessRuleInvalid");
        RuleForEach(x => x.AccessRules)
            .ChildRules(rule =>
            {
                rule.RuleFor(r => r.Operator).IsInEnum().WithErrorCode("PositionAccessRuleInvalid");
            });

        RuleForEach(x => x.Tags)
            .MaximumLength(50)
            .WithErrorCode("ValidationFailed");
    }
}
