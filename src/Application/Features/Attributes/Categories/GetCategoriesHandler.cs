using Domain.Interfaces.Database;
using Domain.ResultPattern.Result;
using Domain.Specifications.Attributes;
using MediatR;

namespace Application.Features.Attributes.Categories;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, Result<List<string>>>
{
    private readonly IAttributeRepository _attributes;

    public GetCategoriesHandler(IAttributeRepository attributes)
    {
        _attributes = attributes;
    }

    public async Task<Result<List<string>>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken
    )
    {
        var categories = await _attributes.ListAsync(
            new AttributeCategoriesSpec(),
            cancellationToken
        );

        return Result<List<string>>.Success(
            categories.Distinct().ToList()
        );
    }
}
