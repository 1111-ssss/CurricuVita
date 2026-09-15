using Ardalis.Specification;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Cvs;
using Domain.Specifications.Positions;
using Domain.Specifications.Profile;
using MediatR;

namespace Application.Features.Cvs.CreateCv;

public class CreateCvHandler : IRequestHandler<CreateCvCommand, Result<int>>
{
    private readonly IPositionRepository _positions;
    private readonly IRepositoryBase<CV> _cvs;
    private readonly IRepositoryBase<UserAttributeValue> _values;

    public CreateCvHandler(
        IPositionRepository positions,
        IRepositoryBase<CV> cvs,
        IRepositoryBase<UserAttributeValue> values
    )
    {
        _positions = positions;
        _cvs = cvs;
        _values = values;
    }

    public async Task<Result<int>> Handle(CreateCvCommand request, CancellationToken cancellationToken)
    {
        var position = await _positions.SingleOrDefaultAsync(
            new PositionByIdSpec(request.PositionId), cancellationToken
        );
        if (position is null)
        {
            return Result.Failure(Errors.PositionNotFound);
        }

        var existing = await _cvs.SingleOrDefaultAsync(
            new CvByUserPositionSpec(request.UserId, request.PositionId), cancellationToken
        );
        if (existing is not null)
        {
            return Result.Failure(Errors.CvAlreadyExists);
        }

        var values = await _values.ListAsync(
            new UserAttributeValuesByUserSpec(request.UserId), cancellationToken
        );
        if (!PositionAccessEvaluator.HasAccess(position, values))
        {
            return Result.Failure(Errors.CvAccessDenied);
        }

        var now = DateTime.UtcNow;
        var cv = new CV
        {
            UserId = request.UserId,
            PositionId = request.PositionId,
            Status = CvStatus.Draft,
            CreatedAt = now,
            UpdatedAt = now,
            Version = 1
        };

        await _cvs.AddAsync(cv, cancellationToken);
        await _cvs.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(cv.Id);
    }
}
