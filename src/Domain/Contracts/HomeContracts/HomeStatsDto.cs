namespace Domain.Contracts.HomeContracts;

public record HomeStatsDto(
    int TotalPositions,
    int TotalCandidates,
    int TotalRecruiters,
    int SubmittedCvs,
    int CvsLast24h
);
