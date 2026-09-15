using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Cvs.CreateCv;

public record CreateCvCommand(
    int UserId,
    int PositionId
) : IRequest<Result<int>>;
