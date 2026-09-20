using Domain.Contracts.HomeContracts;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Home.GetDashboard;

public record GetHomeDashboardQuery : IRequest<Result<HomeDashboardDto>>;
