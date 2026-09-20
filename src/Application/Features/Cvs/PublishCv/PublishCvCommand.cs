using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Cvs.PublishCv;

public record PublishCvCommand(
    int CvId,
    int RequesterUserId,
    bool IsAdmin,
    int ExpectedVersion
) : IRequest<Result>;
