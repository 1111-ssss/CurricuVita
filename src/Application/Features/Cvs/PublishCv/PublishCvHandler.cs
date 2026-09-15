using Ardalis.Specification;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Cvs;
using Domain.Specifications.Profile;
using MediatR;

namespace Application.Features.Cvs.PublishCv;

public class PublishCvHandler : IRequestHandler<PublishCvCommand, Result>
{
    private readonly IRepositoryBase<CV> _cvs;
    private readonly IRepositoryBase<UserAttributeValue> _values;

    public PublishCvHandler(
        IRepositoryBase<CV> cvs,
        IRepositoryBase<UserAttributeValue> values
    )
    {
        _cvs = cvs;
        _values = values;
    }

    public async Task<Result> Handle(PublishCvCommand request, CancellationToken cancellationToken)
    {
        var cv = await _cvs.SingleOrDefaultAsync(new CvByIdSpec(request.CvId), cancellationToken);
        if (cv is null)
        {
            return Result.Failure(Errors.CvNotFound);
        }

        var isOwner = cv.UserId == request.RequesterUserId;
        if (!isOwner && !request.IsAdmin)
        {
            return Result.Failure(Errors.CvForbidden);
        }

        if (cv.Version != request.ExpectedVersion)
        {
            return Result.Failure(Errors.ConcurrencyConflict);
        }

        if (cv.Status == CvStatus.Published)
        {
            return Result.Success();
        }

        var values = await _values.ListAsync(
            new UserAttributeValuesByUserSpec(cv.UserId), cancellationToken
        );
        var byAttribute = values.ToDictionary(v => v.AttributeDefinitionId, v => v);

        foreach (var pa in cv.Position.RequiredAttributes.Where(a => a.IsRequired))
        {
            byAttribute.TryGetValue(pa.AttributeDefinitionId, out var val);
            if (!CvValueHelper.IsFilled(pa.AttributeDefinition.DataType, val))
            {
                return Result.Failure(Errors.CvNotReady);
            }
        }

        cv.Status = CvStatus.Published;
        cv.UpdatedAt = DateTime.UtcNow;
        cv.Version = request.ExpectedVersion + 1;

        try
        {
            await _cvs.UpdateAsync(cv, cancellationToken);
            await _cvs.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex.GetType().Name.Contains("Concurrency"))
        {
            return Result.Failure(Errors.ConcurrencyConflict);
        }

        return Result.Success();
    }
}
