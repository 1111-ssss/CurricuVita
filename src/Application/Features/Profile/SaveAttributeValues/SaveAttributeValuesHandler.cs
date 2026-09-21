using AutoMapper;
using Domain.Contracts.AttributeContracts;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Profile;
using MediatR;

namespace Application.Features.Profile.SaveAttributeValues;

public class SaveAttributeValuesHandler : IRequestHandler<SaveAttributeValuesCommand, Result<List<ProfileAttributeValueDto>>>
{
    private readonly IUserAttributeValueRepository _values;
    private readonly IMapper _mapper;

    public SaveAttributeValuesHandler(
        IUserAttributeValueRepository values,
        IMapper mapper
    )
    {
        _values = values;
        _mapper = mapper;
    }

    public async Task<Result<List<ProfileAttributeValueDto>>> Handle(
        SaveAttributeValuesCommand request,
        CancellationToken cancellationToken
    )
    {
        if (request.UserId != request.RequesterUserId && !request.IsAdmin)
        {
            return Result<List<ProfileAttributeValueDto>>.Failure(Errors.ProfileForbidden);
        }

        if (request.Items.Count == 0)
        {
            var current = await _values.ListAsync(new UserAttributeValuesByUserSpec(request.UserId), cancellationToken);

            return Result<List<ProfileAttributeValueDto>>.Success(
                _mapper.Map<List<ProfileAttributeValueDto>>(current)
            );
        }

        var result = await _values.TrySaveAttributeValuesAsync(request.UserId, request.Items, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<List<ProfileAttributeValueDto>>.Failure(result.Error!);
        }

        return Result<List<ProfileAttributeValueDto>>.Success(
            _mapper.Map<List<ProfileAttributeValueDto>>(result.Value)
        );
    }
}
