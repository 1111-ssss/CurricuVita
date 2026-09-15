using Ardalis.Specification;
using Domain.Contracts.CvContracts;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Cvs;
using Domain.Specifications.Profile;
using MediatR;

namespace Application.Features.Cvs.GetCvDetail;

public class GetCvDetailHandler : IRequestHandler<GetCvDetailQuery, Result<CvDetailDto>>
{
    private readonly IRepositoryBase<CV> _cvs;
    private readonly IRepositoryBase<UserAttributeValue> _values;
    private readonly IProjectRepository _projects;

    public GetCvDetailHandler(
        IRepositoryBase<CV> cvs,
        IRepositoryBase<UserAttributeValue> values,
        IProjectRepository projects
    )
    {
        _cvs = cvs;
        _values = values;
        _projects = projects;
    }

    public async Task<Result<CvDetailDto>> Handle(GetCvDetailQuery request, CancellationToken cancellationToken)
    {
        var cv = await _cvs.SingleOrDefaultAsync(new CvByIdSpec(request.CvId), cancellationToken);
        if (cv is null)
        {
            return Result<CvDetailDto>.Failure(Errors.CvNotFound);
        }

        var isOwner = cv.UserId == request.RequesterUserId;
        var isAdmin = request.IsAdmin;

        var values = await _values.ListAsync(
            new UserAttributeValuesByUserSpec(cv.UserId), cancellationToken
        );
        var byAttribute = values.ToDictionary(v => v.AttributeDefinitionId, v => v);

        var position = cv.Position;
        var isHidden = !PositionAccessEvaluator.HasAccess(position, values);

        if (!isOwner && !isAdmin)
        {
            if (cv.Status != Domain.Enums.CvStatus.Published || isHidden)
            {
                return Result<CvDetailDto>.Failure(Errors.CvAccessDenied);
            }
        }

        var attributes = position.RequiredAttributes
            .OrderBy(a => a.Order)
            .Select(pa =>
            {
                var def = pa.AttributeDefinition;
                byAttribute.TryGetValue(pa.AttributeDefinitionId, out var val);
                var filled = CvValueHelper.IsFilled(def.DataType, val);
                return new CvAttributeDto(
                    pa.AttributeDefinitionId,
                    val?.Id,
                    def.Name,
                    def.Category,
                    def.Description,
                    def.DataType,
                    AttributeOptionsHelper.DeserializeOptions(def.OptionsJson),
                    val?.Version,
                    val?.StringValue,
                    val?.TextValue,
                    val?.ImageValue,
                    val?.NumericValue,
                    val?.DateValue,
                    val?.PeriodStartValue,
                    val?.PeriodEndValue,
                    val?.BooleanValue,
                    def.DataType == Domain.Enums.AttributeDataType.Dropdown ? val?.StringValue : null,
                    pa.IsRequired,
                    pa.Order,
                    filled
                );
            }).ToList();

        var allProjects = await _projects.GetProjectsAsync(cv.UserId, cancellationToken);
        var requiredTags = position.RequiredTags
            .Where(t => t.Tag != null && !string.IsNullOrWhiteSpace(t.Tag.Name))
            .Select(t => t.Tag!.Name)
            .ToList();
        var filtered = CvProjectFilter.Filter(allProjects, requiredTags, position.MaxProjectCount);

        var canEdit = isOwner || isAdmin;
        var canPublish = canEdit
            && cv.Status == Domain.Enums.CvStatus.Draft
            && attributes.Where(a => a.IsRequired).All(a => a.IsFilled);

        var candidateName = $"{cv.User?.FirstName} {cv.User?.LastName}".Trim();
        if (string.IsNullOrWhiteSpace(candidateName))
        {
            candidateName = cv.User?.Email ?? $"User {cv.UserId}";
        }

        return Result<CvDetailDto>.Success(new CvDetailDto(
            cv.Id,
            cv.UserId,
            candidateName,
            cv.PositionId,
            position.Title,
            position.DescriptionMarkdown,
            cv.Status,
            cv.Version,
            cv.CreatedAt,
            cv.UpdatedAt,
            canEdit,
            canPublish,
            isHidden,
            attributes,
            filtered.Select(p => new CvProjectDto(
                p.Id,
                p.Title,
                p.StartDate,
                p.EndDate,
                p.DescriptionMarkdown,
                p.Tags.Where(t => t.Tag != null).Select(t => t.Tag!.Name).OrderBy(n => n).ToList(),
                p.Version)
            ).ToList(),
            requiredTags.OrderBy(t => t).ToList(),
            position.MaxProjectCount,
            cv.Likes.Count)
        );
    }
}
