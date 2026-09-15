using Ardalis.Specification;
using Domain.Entities;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Cvs;
using MediatR;

namespace Application.Features.Likes.ToggleLike;

public class ToggleLikeCvHandler : IRequestHandler<ToggleLikeCvCommand, Result<ToggleLikeCvResponse>>
{
    private readonly IRepositoryBase<CV> _cvs;
    private readonly IRepositoryBase<Like> _likes;

    public ToggleLikeCvHandler(
        IRepositoryBase<CV> cvs,
        IRepositoryBase<Like> likes
    )
    {
        _cvs = cvs;
        _likes = likes;
    }

    public async Task<Result<ToggleLikeCvResponse>> Handle(ToggleLikeCvCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsRecruiter && !request.IsAdmin)
        {
            return Result<ToggleLikeCvResponse>.Failure(Errors.LikeForbidden);
        }

        var cv = await _cvs.SingleOrDefaultAsync(new CvByIdSpec(request.CvId), cancellationToken);
        if (cv is null)
        {
            return Result<ToggleLikeCvResponse>.Failure(Errors.CvNotFound);
        }

        var existing = cv.Likes.FirstOrDefault(l => l.RecruiterId == request.RequesterUserId);
        bool isLiked;

        if (existing is not null)
        {
            await _likes.DeleteAsync(existing, cancellationToken);
            isLiked = false;
        }
        else
        {
            await _likes.AddAsync(new Like
            {
                CVId = cv.Id,
                RecruiterId = request.RequesterUserId,
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);
            isLiked = true;
        }

        await _likes.SaveChangesAsync(cancellationToken);

        var fresh = await _cvs.SingleOrDefaultAsync(new CvByIdSpec(request.CvId), cancellationToken);
        return Result<ToggleLikeCvResponse>.Success(new ToggleLikeCvResponse(isLiked, fresh?.Likes.Count ?? 0));
    }
}
