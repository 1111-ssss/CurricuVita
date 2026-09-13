using Domain.Contracts.PositionContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Positions.UpdatePosition;

public record UpdatePositionCommand(
    int Id,
    int Version,
    string Title,
    string DescriptionMarkdown,
    bool IsPublic,
    int? MaxProjectCount,
    List<PositionAttributeInput> Attributes,
    List<PositionAccessRuleInput> AccessRules,
    List<string> Tags
) : IRequest<Result<PositionDetailDto>>;
