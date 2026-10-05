using Domain.Contracts.PositionContracts;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Database;
using Domain.Interfaces.Services;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Helpers;
using Domain.Specifications.Positions;
using Domain.Specifications.Profile;
using Ardalis.Specification;
using MediatR;

namespace Application.Features.Integrations.GetPositionExternalExport;

public class GetPositionExternalExportHandler : IRequestHandler<GetPositionExternalExportQuery, Result<PositionExternalExportDto>>
{
    private readonly IPositionApiTokenService _tokens;
    private readonly IPositionRepository _positions;
    private readonly IRepositoryBase<CV> _cvs;
    private readonly IRepositoryBase<UserAttributeValue> _values;

    public GetPositionExternalExportHandler(
        IPositionApiTokenService tokens,
        IPositionRepository positions,
        IRepositoryBase<CV> cvs,
        IRepositoryBase<UserAttributeValue> values
    )
    {
        _tokens = tokens;
        _positions = positions;
        _cvs = cvs;
        _values = values;
    }

    public async Task<Result<PositionExternalExportDto>> Handle(
        GetPositionExternalExportQuery request,
        CancellationToken cancellationToken
    )
    {
        if (!_tokens.TryResolve(request.ApiToken, out var positionId))
        {
            return Result<PositionExternalExportDto>.Failure(Errors.Unauthorized);
        }

        var position = await _positions.SingleOrDefaultAsync(
            new PositionByIdSpec(positionId), cancellationToken);
        if (position is null)
        {
            return Result<PositionExternalExportDto>.Failure(Errors.PositionNotFound);
        }

        var published = await _cvs.ListAsync(new PositionCvsSpec(positionId), cancellationToken);
        var userIds = published.Select(c => c.UserId).Distinct().ToList();

        var attributes = position.RequiredAttributes
            .Select(a => new PositionExternalAttributeDto(
                a.AttributeDefinitionId,
                a.AttributeDefinition?.Name ?? $"#{a.AttributeDefinitionId}",
                a.AttributeDefinition?.Category ?? string.Empty,
                (a.AttributeDefinition?.DataType ?? AttributeDataType.String).ToString(),
                a.IsRequired))
            .OrderBy(a => a.Name)
            .ToList();

        if (userIds.Count == 0)
        {
            return Result<PositionExternalExportDto>.Success(new PositionExternalExportDto(
                position.Id,
                position.Title,
                position.Company,
                position.Level,
                published.Count,
                attributes,
                new(),
                new()));
        }

        var attrIds = position.RequiredAttributes.Select(a => a.AttributeDefinitionId).ToList();
        var values = await _values.ListAsync(
            new UserAttributeValuesByUsersSpec(userIds, attrIds), cancellationToken);

        var names = position.RequiredAttributes.ToDictionary(
            a => a.AttributeDefinitionId,
            a => a.AttributeDefinition?.Name ?? $"#{a.AttributeDefinitionId}");
        var defs = position.RequiredAttributes.ToDictionary(
            a => a.AttributeDefinitionId,
            a => (a.AttributeDefinition?.Name ?? $"#{a.AttributeDefinitionId}",
                  a.AttributeDefinition?.DataType ?? AttributeDataType.String));

        return Result<PositionExternalExportDto>.Success(new PositionExternalExportDto(
            position.Id,
            position.Title,
            position.Company,
            position.Level,
            published.Count,
            attributes,
            PositionAggregation.Numeric(values, names),
            PositionAggregation.TextTop(values, defs)));
    }
}
