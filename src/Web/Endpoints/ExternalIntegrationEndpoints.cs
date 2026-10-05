using Application.Features.Integrations.GetPositionExternalExport;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Endpoints;

public static class ExternalIntegrationEndpoints
{
    public static IEndpointRouteBuilder MapExternalIntegrationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/external");

        group.MapGet("positions/{apiToken}", GetPositionExport);

        return endpoints;
    }

    private static async Task<IResult> GetPositionExport(
        [FromServices] IMediator mediator,
        string apiToken,
        CancellationToken cancellationToken = default
    )
    {
        var result = await mediator.Send(new GetPositionExternalExportQuery(apiToken), cancellationToken);

        return result.ToMinimalApiResult();
    }
}
