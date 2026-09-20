using Domain.Contracts.PositionContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Positions.CreatePosition;

public record CreatePositionCommand(
    string Title,
    string DescriptionMarkdown,
    string? Company,
    string? Level,
    bool IsPublic,
    int? MaxProjectCount,
    List<PositionAttributeInput> Attributes,
    List<PositionAccessRuleInput> AccessRules,
    List<string> Tags
) : IRequest<Result<PositionDetailDto>>;
