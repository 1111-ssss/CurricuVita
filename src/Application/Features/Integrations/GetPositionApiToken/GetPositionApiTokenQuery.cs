using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Integrations.GetPositionApiToken;

public record GetPositionApiTokenQuery(
    int PositionId
) : IRequest<Result<string>>;
