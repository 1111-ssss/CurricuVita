using Ardalis.Specification;
using Domain.Entities;
using Domain.ResultPattern.Result;
using Domain.Specifications.Profile;
using MediatR;

namespace Application.Features.Tags.GetTagSuggestions;

public class GetTagSuggestionsHandler : IRequestHandler<GetTagSuggestionsQuery, Result<List<string>>>
{
    private readonly IRepositoryBase<Tag> _tags;

    public GetTagSuggestionsHandler(IRepositoryBase<Tag> tags)
    {
        _tags = tags;
    }

    public async Task<Result<List<string>>> Handle(GetTagSuggestionsQuery request, CancellationToken cancellationToken)
    {
        var take = Math.Clamp(request.Take, 1, 50);
        var items = await _tags.ListAsync(new TagSuggestionsSpec(request.Prefix, take), cancellationToken);

        return Result<List<string>>.Success(items);
    }
}
