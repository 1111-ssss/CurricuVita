using FluentValidation;

namespace Application.Features.Positions.DuplicatePosition;

public class DuplicatePositionCommandValidator : AbstractValidator<DuplicatePositionCommand>
{
    public DuplicatePositionCommandValidator()
    {
        RuleFor(x => x.SourcePositionId).GreaterThan(0);
    }
}
