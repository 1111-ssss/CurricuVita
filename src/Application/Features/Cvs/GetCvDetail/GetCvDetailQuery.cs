using Domain.Contracts.CvContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Cvs.GetCvDetail;

public record GetCvDetailQuery(
    int CvId,
    int RequesterUserId,
    bool IsAdmin,
    bool IsRecruiter = false
) : IRequest<Result<CvDetailDto>>;
