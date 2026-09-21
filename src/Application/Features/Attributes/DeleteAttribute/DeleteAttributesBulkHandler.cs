using Domain.Interfaces.Database;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Attributes.DeleteAttribute;

public class DeleteAttributesBulkHandler : IRequestHandler<DeleteAttributesBulkCommand, Result<int>>
{
    private readonly IAttributeRepository _attributes;

    public DeleteAttributesBulkHandler(IAttributeRepository attributes)
    {
        _attributes = attributes;
    }

    public async Task<Result<int>> Handle(
        DeleteAttributesBulkCommand request,
        CancellationToken cancellationToken
    )
    {
        var ids = request.Ids?.Distinct().ToList() ?? new List<int>();
        if (ids.Count == 0)
        {
            return Result<int>.Success(0);
        }

        var deleted = await _attributes.DeleteAttributesBulkAsync(ids, cancellationToken);
        return Result<int>.Success(deleted);
    }
}
