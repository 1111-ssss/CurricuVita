namespace Domain.Contracts.HomeContracts;

public record HomeDashboardDto(
    List<HomePositionItemDto> LatestPositions,
    List<HomePositionItemDto> TopPositions,
    List<TagCloudItemDto> TagCloud,
    HomeStatsDto Stats
);