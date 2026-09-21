using AutoMapper;
using Domain.Contracts.PositionContracts;
using Domain.Entities;
using Domain.Interfaces.Database;
using Domain.Interfaces.Identity;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Positions;
using MediatR;

namespace Application.Features.Positions.DuplicatePosition;

public class DuplicatePositionHandler : IRequestHandler<DuplicatePositionCommand, Result<PositionDetailDto>>
{
    private readonly IPositionRepository _positions;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public DuplicatePositionHandler(
        IPositionRepository positions,
        ICurrentUserService currentUser,
        IMapper mapper
    )
    {
        _positions = positions;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<Result<PositionDetailDto>> Handle(
        DuplicatePositionCommand request,
        CancellationToken cancellationToken
    )
    {
        if (_currentUser.UserId is not int createdById)
        {
            return Result<PositionDetailDto>.Failure(Errors.Unauthorized);
        }

        var source = await _positions.SingleOrDefaultAsync(
            new PositionByIdSpec(request.SourcePositionId), cancellationToken
        );
        if (source is null)
        {
            return Result<PositionDetailDto>.Failure(Errors.PositionNotFound);
        }

        var now = DateTime.UtcNow;
        var copy = new Position
        {
            Title = source.Title + " (copy)",
            DescriptionMarkdown = source.DescriptionMarkdown,
            Company = source.Company,
            Level = source.Level,
            IsPublic = source.IsPublic,
            MaxProjectCount = source.MaxProjectCount,
            CreatedAt = now,
            UpdatedAt = now,
            Version = 1,
            CreatedById = createdById
        };

        foreach (var a in source.RequiredAttributes.OrderBy(a => a.Order))
        {
            copy.RequiredAttributes.Add(new PositionAttribute
            {
                AttributeDefinitionId = a.AttributeDefinitionId,
                IsRequired = a.IsRequired,
                Order = a.Order
            });
        }

        foreach (var r in source.AccessRules)
        {
            copy.AccessRules.Add(new PositionAccessRule
            {
                AttributeDefinitionId = r.AttributeDefinitionId,
                Operator = r.Operator,
                Value = r.Value
            });
        }

        foreach (var t in source.RequiredTags)
        {
            copy.RequiredTags.Add(new PositionTag { TagId = t.TagId });
        }

        await _positions.AddAsync(copy, cancellationToken);
        await _positions.SaveChangesAsync(cancellationToken);

        var reloaded = await _positions.SingleOrDefaultAsync(
            new PositionByIdSpec(copy.Id), cancellationToken
        );

        return reloaded is null
            ? Result<PositionDetailDto>.Failure(Errors.PositionNotFound)
            : Result<PositionDetailDto>.Success(_mapper.Map<PositionDetailDto>(reloaded));
    }
}
