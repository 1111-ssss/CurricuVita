using Domain.Contracts.CvContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Cvs.GetCvDetail;

public record GetCvDetailQuery(
    int CvId,
    int RequesterUserId,
    bool IsAdmin,
    bool CanLike = false
) : IRequest<Result<CvDetailDto>>;
