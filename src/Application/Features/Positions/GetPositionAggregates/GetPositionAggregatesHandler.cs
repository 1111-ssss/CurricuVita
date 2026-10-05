using Ardalis.Specification;
using Domain.Contracts.PositionContracts;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Positions;
using Domain.Specifications.Profile;
using Domain.Helpers;
using MediatR;

namespace Application.Features.Positions.GetPositionAggregates;

public class GetPositionAggregatesHandler : IRequestHandler<GetPositionAggregatesQuery, Result<PositionAggregatesDto>>
{
    private readonly IPositionRepository _positions;
    private readonly IRepositoryBase<CV> _cvs;
    private readonly IRepositoryBase<UserAttributeValue> _values;

    public GetPositionAggregatesHandler(
        IPositionRepository positions,
        IRepositoryBase<CV> cvs,
        IRepositoryBase<UserAttributeValue> values
    )
    {
        _positions = positions;
        _cvs = cvs;
        _values = values;
    }

    public async Task<Result<PositionAggregatesDto>> Handle(
        GetPositionAggregatesQuery request,
        CancellationToken cancellationToken
    )
    {
        var position = await _positions.SingleOrDefaultAsync(
            new PositionByIdSpec(request.PositionId), cancellationToken);
        if (position is null)
        {
            return Result<PositionAggregatesDto>.Failure(Errors.PositionNotFound);
        }

        var numericAttrs = position.RequiredAttributes
            .Where(a => a.AttributeDefinition?.DataType == AttributeDataType.Numeric)
            .ToList();

        var published = await _cvs.ListAsync(
            new PositionCvsSpec(request.PositionId), cancellationToken);
        var userIds = published.Select(c => c.UserId).Distinct().ToList();

        if (numericAttrs.Count == 0 || userIds.Count == 0)
        {
            return Result<PositionAggregatesDto>.Success(
                new PositionAggregatesDto(request.PositionId, published.Count, new()));
        }

        var attrIds = numericAttrs.Select(a => a.AttributeDefinitionId).ToList();
        var values = await _values.ListAsync(
            new UserAttributeValuesByUsersSpec(userIds, attrIds), cancellationToken);

        var names = numericAttrs.ToDictionary(
            a => a.AttributeDefinitionId,
            a => a.AttributeDefinition?.Name ?? $"#{a.AttributeDefinitionId}");
        var aggregates = PositionAggregation.Numeric(values, names);

        return Result<PositionAggregatesDto>.Success(
            new PositionAggregatesDto(request.PositionId, published.Count, aggregates)
        );
    }
}
