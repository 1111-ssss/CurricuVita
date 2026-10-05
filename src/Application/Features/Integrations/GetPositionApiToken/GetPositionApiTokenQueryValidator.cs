using FluentValidation;

namespace Application.Features.Integrations.GetPositionApiToken;

public class GetPositionApiTokenQueryValidator : AbstractValidator<GetPositionApiTokenQuery>
{
    public GetPositionApiTokenQueryValidator()
    {
        RuleFor(x => x.PositionId)
            .GreaterThan(0)
            .WithMessage("Position is required.")
            .WithErrorCode("PositionNotFound");
    }
}
