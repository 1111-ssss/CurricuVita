using Domain.Helpers;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Attributes.DeleteAttribute;

public class DeleteAttributeHandler : IRequestHandler<DeleteAttributeCommand, Result>
{
    private readonly IAttributeRepository _attributes;

    public DeleteAttributeHandler(IAttributeRepository attributes)
    {
        _attributes = attributes;
    }

    public async Task<Result> Handle(
        DeleteAttributeCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _attributes.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return Result.Failure(Errors.AttributeNotFound);
        }

        if (string.Equals(entity.Category, AttributeOptionsHelper.ProtectedCategory, StringComparison.OrdinalIgnoreCase))
        {
            return Result.Failure(Errors.AttributeProtected);
        }

        await _attributes.DeleteAsync(entity, cancellationToken);
        await _attributes.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
