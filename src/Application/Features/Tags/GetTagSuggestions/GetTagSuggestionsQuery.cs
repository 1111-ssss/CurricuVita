using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Tags.GetTagSuggestions;

public record GetTagSuggestionsQuery(
    string? Prefix,
    int Take = 10
) : IRequest<Result<List<string>>>;
