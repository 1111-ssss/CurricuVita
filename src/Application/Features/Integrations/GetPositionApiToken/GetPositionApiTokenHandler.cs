using Domain.Interfaces.Database;
using Domain.Interfaces.Services;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Positions;
using MediatR;

namespace Application.Features.Integrations.GetPositionApiToken;

public class GetPositionApiTokenHandler : IRequestHandler<GetPositionApiTokenQuery, Result<string>>
{
    private readonly IPositionRepository _positions;
    private readonly IPositionApiTokenService _tokens;

    public GetPositionApiTokenHandler(
        IPositionRepository positions,
        IPositionApiTokenService tokens
    )
    {
        _positions = positions;
        _tokens = tokens;
    }

    public async Task<Result<string>> Handle(
        GetPositionApiTokenQuery request,
        CancellationToken cancellationToken
    )
    {
        var exists = await _positions.AnyAsync(
            new PositionExistsSpec(request.PositionId),
            cancellationToken
        );
        if (!exists)
        {
            return Result<string>.Failure(Errors.PositionNotFound);
        }

        return Result<string>.Success(_tokens.Create(request.PositionId));
    }
}
