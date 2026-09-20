using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Likes.ToggleLike;

public record ToggleLikeCvCommand(
    int CvId,
    int RequesterUserId,
    bool IsRecruiter,
    bool IsAdmin = false
) : IRequest<Result<ToggleLikeCvResponse>>;