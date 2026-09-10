using Domain.Contracts;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Attributes;
using MediatR;

namespace Application.Features.Attributes.CreateAttribute;

public class CreateAttributeHandler : IRequestHandler<CreateAttributeCommand, Result<AttributeDto>>
{
    private readonly IAttributeRepository _attributes;

    public CreateAttributeHandler(IAttributeRepository attributes)
    {
        _attributes = attributes;
    }

    public async Task<Result<AttributeDto>> Handle(
        CreateAttributeCommand request,
        CancellationToken cancellationToken
    )
    {
        var command = request with
        {
            Category = request.Category?.Trim() ?? string.Empty,
            Name = request.Name?.Trim() ?? string.Empty,
            Description = request.Description?.Trim() ?? string.Empty
        };

        var normalizedOptions = AttributeOptionsHelper.NormalizeOptions(command.DataType, command.Options);

        if (await _attributes.AnyAsync(new AttributeNameExistsSpec(command.Name), cancellationToken))
        {
            return Result<AttributeDto>.Failure(Errors.AttributeNameDuplicate);
        }

        var entity = new AttributeDefinition
        {
            Category = command.Category,
            Name = command.Name,
            Description = command.Description,
            DataType = command.DataType,
            OptionsJson = AttributeOptionsHelper.SerializeOptions(normalizedOptions),
            CreatedAt = DateTime.UtcNow,
            Version = 1
        };

        await _attributes.AddAsync(entity, cancellationToken);
        await _attributes.SaveChangesAsync(cancellationToken);

        return Result<AttributeDto>.Success(new AttributeDto(
            entity.Id,
            entity.Category,
            entity.Name,
            entity.Description,
            entity.DataType,
            normalizedOptions,
            entity.Version,
            entity.CreatedAt,
            0
        ));
    }
}
