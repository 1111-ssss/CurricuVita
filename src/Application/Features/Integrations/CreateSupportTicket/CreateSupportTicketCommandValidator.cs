using FluentValidation;

namespace Application.Features.Integrations.CreateSupportTicket;

public class CreateSupportTicketCommandValidator : AbstractValidator<CreateSupportTicketCommand>
{
    public CreateSupportTicketCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User is required.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.PositionId)
            .GreaterThan(0)
            .When(x => x.PositionId.HasValue)
            .WithMessage("Position is invalid.")
            .WithErrorCode("PositionNotFound");
        RuleFor(x => x.Link)
            .NotEmpty()
            .WithMessage("Link is required.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Link)
            .MaximumLength(2000)
            .WithMessage("Link must be at most 2000 characters.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Summary)
            .NotEmpty()
            .WithMessage("Summary is required.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Summary)
            .MaximumLength(2000)
            .WithMessage("Summary must be at most 2000 characters.")
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Priority is invalid.")
            .WithErrorCode("ValidationFailed");
    }
}
