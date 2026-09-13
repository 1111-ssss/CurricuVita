using Ardalis.Specification;
using AutoMapper;
using Domain.Contracts.PositionContracts;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces.Database;
using Domain.Interfaces.Identity;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Attributes;
using Domain.Specifications.Positions;
using Domain.Specifications.Tags;
using MediatR;

namespace Application.Features.Positions.CreatePosition;

public class CreatePositionHandler : IRequestHandler<CreatePositionCommand, Result<PositionDetailDto>>
{
    private readonly IPositionRepository _positions;
    private readonly IAttributeRepository _attributes;
    private readonly IRepositoryBase<Tag> _tags;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public CreatePositionHandler(
        IPositionRepository positions,
        IAttributeRepository attributes,
        IRepositoryBase<Tag> tags,
        ICurrentUserService currentUser,
        IMapper mapper
    )
    {
        _positions = positions;
        _attributes = attributes;
        _tags = tags;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<Result<PositionDetailDto>> Handle(
        CreatePositionCommand request,
        CancellationToken cancellationToken
    )
    {
        if (_currentUser.UserId is not int createdById)
        {
            return Result<PositionDetailDto>.Failure(Errors.Unauthorized);
        }

        var title = request.Title?.Trim() ?? string.Empty;
        var description = request.DescriptionMarkdown?.Trim() ?? string.Empty;
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

        var now = DateTime.UtcNow;
        var entity = new Position
        {
            Title = title,
            DescriptionMarkdown = description,
            IsPublic = request.IsPublic,
            MaxProjectCount = request.MaxProjectCount,
            CreatedAt = now,
            UpdatedAt = now,
            Version = 1,
            CreatedById = createdById
        };

        var order = 0;
        foreach (var a in attributes)
        {
            entity.RequiredAttributes.Add(new PositionAttribute
            {
                AttributeDefinitionId = a.AttributeDefinitionId,
                IsRequired = a.IsRequired,
                Order = order++
            });
        }

        foreach (var r in rules)
        {
            entity.AccessRules.Add(new PositionAccessRule
            {
                AttributeDefinitionId = r.AttributeDefinitionId,
                Operator = r.Operator,
                Value = r.Value.Trim()
            });
        }

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

            entity.RequiredTags.Add(new PositionTag { TagId = tag.Id });
        }

        await _positions.AddAsync(entity, cancellationToken);
        await _positions.SaveChangesAsync(cancellationToken);

        var reloaded = await _positions.SingleOrDefaultAsync(
            new PositionByIdSpec(entity.Id), cancellationToken
        );

        return reloaded is null
            ? Result<PositionDetailDto>.Failure(Errors.PositionNotFound)
            : Result<PositionDetailDto>.Success(_mapper.Map<PositionDetailDto>(reloaded));
    }
}
