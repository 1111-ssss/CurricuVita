using Ardalis.Specification;
using AutoMapper;
using Domain.Contracts.PositionContracts;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Attributes;
using Domain.Specifications.Positions;
using Domain.Specifications.Tags;
using MediatR;

namespace Application.Features.Positions.UpdatePosition;

public class UpdatePositionHandler : IRequestHandler<UpdatePositionCommand, Result<PositionDetailDto>>
{
    private readonly IPositionRepository _positions;
    private readonly IAttributeRepository _attributes;
    private readonly IRepositoryBase<Tag> _tags;
    private readonly IMapper _mapper;

    public UpdatePositionHandler(
        IPositionRepository positions,
        IAttributeRepository attributes,
        IRepositoryBase<Tag> tags,
        IMapper mapper
    )
    {
        _positions = positions;
        _attributes = attributes;
        _tags = tags;
        _mapper = mapper;
    }

    public async Task<Result<PositionDetailDto>> Handle(
        UpdatePositionCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _positions.SingleOrDefaultAsync(
            new PositionByIdSpec(request.Id), cancellationToken
        );

        if (entity is null)
        {
            return Result<PositionDetailDto>.Failure(Errors.PositionNotFound);
        }

        if (entity.Version != request.Version)
        {
            return Result<PositionDetailDto>.Failure(Errors.ConcurrencyConflict);
        }

        var attributes = request.Attributes ?? new();
        var rules = request.AccessRules ?? new();
        var tags = TagHelper.NormalizeTags(request.Tags);

        var referencedIds = attributes.Select(a => a.AttributeDefinitionId)
            .Concat(rules.Select(r => r.AttributeDefinitionId))
            .Distinct()
            .ToList();

        if (referencedIds.Count > 0)
        {
            var existing = await _attributes.ListAsync(
                new AttributesByIdsSpec(referencedIds), cancellationToken
            );
            var existingIds = existing.Select(a => a.Id).ToHashSet();
            if (referencedIds.Any(id => !existingIds.Contains(id)))
            {
                return attributes.Any(a => !existingIds.Contains(a.AttributeDefinitionId))
                    ? Result<PositionDetailDto>.Failure(Errors.PositionAttributeNotFound)
                    : Result<PositionDetailDto>.Failure(Errors.PositionAccessRuleInvalid);
            }
        }

        entity.Title = request.Title.Trim();
        entity.DescriptionMarkdown = request.DescriptionMarkdown?.Trim() ?? string.Empty;
        entity.Company = string.IsNullOrWhiteSpace(request.Company) ? null : request.Company.Trim();
        entity.Level = Domain.Constants.PositionLevels.Normalize(request.Level);
        entity.IsPublic = request.IsPublic;
        entity.MaxProjectCount = request.MaxProjectCount;
        entity.UpdatedAt = DateTime.UtcNow;

        entity.RequiredAttributes.Clear();
        var order = 0;
        foreach (var a in attributes)
        {
            entity.RequiredAttributes.Add(new PositionAttribute
            {
                PositionId = entity.Id,
                AttributeDefinitionId = a.AttributeDefinitionId,
                IsRequired = a.IsRequired,
                Order = order++
            });
        }

        entity.AccessRules.Clear();
        foreach (var r in rules)
        {
            entity.AccessRules.Add(new PositionAccessRule
            {
                PositionId = entity.Id,
                AttributeDefinitionId = r.AttributeDefinitionId,
                Operator = r.Operator,
                Value = r.Value.Trim()
            });
        }

        entity.RequiredTags.Clear();
        foreach (var tagName in tags)
        {
            var tag = await _tags.SingleOrDefaultAsync(
                new TagByNameSpec(tagName), cancellationToken
            );
            if (tag is null)
            {
                tag = new Tag { Name = tagName };
                await _tags.AddAsync(tag, cancellationToken);
                await _tags.SaveChangesAsync(cancellationToken);
            }

            entity.RequiredTags.Add(new PositionTag
            {
                PositionId = entity.Id,
                TagId = tag.Id
            });
        }

        var saved = await _positions.TrySaveWithConcurrencyAsync(
            entity, request.Version, cancellationToken
        );
        if (!saved)
        {
            return Result<PositionDetailDto>.Failure(Errors.ConcurrencyConflict);
        }

        var reloaded = await _positions.SingleOrDefaultAsync(
            new PositionByIdSpec(entity.Id), cancellationToken
        );

        return reloaded is null
            ? Result<PositionDetailDto>.Failure(Errors.PositionNotFound)
            : Result<PositionDetailDto>.Success(_mapper.Map<PositionDetailDto>(reloaded));
    }
}
