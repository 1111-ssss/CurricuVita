using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Cvs.GetMyCv;

public record GetMyCvQuery(
    int UserId,
    int PositionId
) : IRequest<Result<int?>>;
