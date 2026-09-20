using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Attributes.Categories;

public record GetCategoriesQuery() : IRequest<Result<List<string>>>;
