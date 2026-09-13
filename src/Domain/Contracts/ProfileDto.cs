namespace Domain.Contracts;

public record ProfileDto(
    MeDto Me,
    List<ProfileAttributeValueDto> Values,
    List<ProjectDto> Projects,
    List<CvListItemDto> Cvs
);
