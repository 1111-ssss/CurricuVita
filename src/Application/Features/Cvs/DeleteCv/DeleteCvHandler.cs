using Ardalis.Specification;
using Domain.Entities;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Cvs;
using MediatR;

namespace Application.Features.Cvs.DeleteCv;

public class DeleteCvHandler : IRequestHandler<DeleteCvCommand, Result>
{
    private readonly IRepositoryBase<CV> _cvs;

    public DeleteCvHandler(IRepositoryBase<CV> cvs)
    {
        _cvs = cvs;
    }

    public async Task<Result> Handle(DeleteCvCommand request, CancellationToken cancellationToken)
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

        await _cvs.DeleteAsync(cv, cancellationToken);
        try
        {
            await _cvs.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex.GetType().Name.Contains("Concurrency"))
        {
            return Result.Failure(Errors.ConcurrencyConflict);
        }

        return Result.Success();
    }
}
