using FluentValidation;

namespace Application.Features.Discussions.PostMessage;

public class PostDiscussionMessageCommandValidator : AbstractValidator<PostDiscussionMessageCommand>
{
    public PostDiscussionMessageCommandValidator()
    {
        RuleFor(x => x.PositionId)
            .GreaterThan(0)
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.AuthorId)
            .GreaterThan(0)
            .WithErrorCode("ValidationFailed");
        RuleFor(x => x.ContentMarkdown)
            .NotEmpty()
            .MaximumLength(5000)
            .WithErrorCode("DiscussionInvalidContent");
    }
}
