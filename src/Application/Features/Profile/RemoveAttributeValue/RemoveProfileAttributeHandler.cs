using Domain.Interfaces.Database;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Profile.RemoveAttributeValue;

public class RemoveProfileAttributeHandler : IRequestHandler<RemoveProfileAttributeCommand, Result>
{
    private readonly IUserAttributeValueRepository _values;

    public RemoveProfileAttributeHandler(IUserAttributeValueRepository values)
    {
        _values = values;
    }

    public async Task<Result> Handle(RemoveProfileAttributeCommand request, CancellationToken cancellationToken)
    {
        var result = await _values.RemoveAttributeValueAsync(request.UserId, request.ValueId, cancellationToken);

        return result;
    }
}
