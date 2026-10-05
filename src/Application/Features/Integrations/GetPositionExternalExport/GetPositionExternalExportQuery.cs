using Domain.Contracts.PositionContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Integrations.GetPositionExternalExport;

public record GetPositionExternalExportQuery(
    string ApiToken
) : IRequest<Result<PositionExternalExportDto>>;
