using Ardalis.Specification;
using Domain.Entities;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Result;
using Domain.Specifications.Cvs;
using MediatR;

namespace Application.Features.Cvs.GetMyCv;

public class GetMyCvHandler : IRequestHandler<GetMyCvQuery, Result<int?>>
{
    private readonly IRepositoryBase<CV> _cvs;

    public GetMyCvHandler(IRepositoryBase<CV> cvs)
    {
        _cvs = cvs;
    }

    public async Task<Result<int?>> Handle(GetMyCvQuery request, CancellationToken cancellationToken)
    {
        var cv = await _cvs.SingleOrDefaultAsync(
            new CvByUserPositionSpec(request.UserId, request.PositionId), cancellationToken
        );
        return Result<int?>.Success(cv?.Id);
    }
}
