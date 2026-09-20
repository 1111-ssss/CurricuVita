namespace Domain.Contracts.UserContracts;

public record PublicProfileDto(
    int UserId,
    string DisplayName,
    string Location,
    string? AvatarUrl,
    List<PublicProfileCvDto> PublishedCvs
);
