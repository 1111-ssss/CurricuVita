using Domain.Contracts.AttributeContracts;

namespace Domain.Contracts.UserContracts;

public record ProfileDto(
    MeDto Me,
    List<ProfileAttributeValueDto> Values,
    List<ProjectDto> Projects,
    List<CvListItemDto> Cvs
);
