using AutoMapper;
using Domain.Contracts.PositionContracts;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Positions;
using MediatR;

namespace Application.Features.Positions.GetPosition;

public class GetPositionHandler : IRequestHandler<GetPositionQuery, Result<PositionDetailDto>>
{
    private readonly IPositionRepository _positions;
    private readonly IMapper _mapper;

    public GetPositionHandler(
        IPositionRepository positions,
        IMapper mapper
    )
    {
        _positions = positions;
        _mapper = mapper;
    }

    public async Task<Result<PositionDetailDto>> Handle(
        GetPositionQuery request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _positions.SingleOrDefaultAsync(
            new PositionByIdSpec(request.Id),
            cancellationToken
        );

        if (entity is null)
        {
            return Result<PositionDetailDto>.Failure(Errors.PositionNotFound);
        }

        return Result<PositionDetailDto>.Success(_mapper.Map<PositionDetailDto>(entity));
    }
}
