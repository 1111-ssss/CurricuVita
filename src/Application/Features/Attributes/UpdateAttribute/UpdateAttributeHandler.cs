using Domain.Contracts.AttributeContracts;
using Domain.Helpers;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Attributes;
using MediatR;

namespace Application.Features.Attributes.UpdateAttribute;

public class UpdateAttributeHandler : IRequestHandler<UpdateAttributeCommand, Result<AttributeDto>>
{
    private readonly IAttributeRepository _attributes;

    public UpdateAttributeHandler(IAttributeRepository attributes)
    {
        _attributes = attributes;
    }

    public async Task<Result<AttributeDto>> Handle(
        UpdateAttributeCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _attributes.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return Result<AttributeDto>.Failure(Errors.AttributeNotFound);
        }

        var command = request with
        {
            Category = request.Category?.Trim() ?? string.Empty,
            Name = request.Name?.Trim() ?? string.Empty,
            Description = request.Description?.Trim() ?? string.Empty
        };

        if (entity.DataType != command.DataType)
        {
            return Result<AttributeDto>.Failure(Errors.ValidationFailed);
        }

        if (await _attributes.AnyAsync(
                new AttributeNameExistsSpec(command.Name, excludeId: request.Id),
                cancellationToken)
            )
        {
            return Result<AttributeDto>.Failure(Errors.AttributeNameDuplicate);
        }

        if (entity.Version != request.Version)
        {
            return Result<AttributeDto>.Failure(Errors.ConcurrencyConflict);
        }

        entity.Category = command.Category;
        entity.Name = command.Name;
        entity.Description = command.Description;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.OptionsJson = AttributeOptionsHelper.SerializeOptions(
            AttributeOptionsHelper.NormalizeOptions(entity.DataType, command.Options)
        );

        var saved = await _attributes.TrySaveWithConcurrencyAsync(
            entity, request.Version, cancellationToken
        );
        if (!saved)
        {
            return Result<AttributeDto>.Failure(Errors.ConcurrencyConflict);
        }

        var reloaded = await _attributes.SingleOrDefaultAsync(
            new AttributeByIdSpec(request.Id),
            cancellationToken
        );

        return reloaded is null
            ? Result<AttributeDto>.Failure(Errors.AttributeNotFound)
            : Result<AttributeDto>.Success(reloaded.ToDto());
    }
}
