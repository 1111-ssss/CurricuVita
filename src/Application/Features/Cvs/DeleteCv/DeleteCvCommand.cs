using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Cvs.DeleteCv;

public record DeleteCvCommand(
    int CvId,
    int RequesterUserId,
    bool IsAdmin,
    int ExpectedVersion
) : IRequest<Result>;
