using Domain.Enums;

namespace Domain.Contracts.CvContracts;

public record CvDetailDto(
    int Id,
    int UserId,
    string CandidateName,
    int PositionId,
    string PositionTitle,
    string PositionDescriptionMarkdown,
    CvStatus Status,
    int Version,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool CanEdit,
    bool CanPublish,
    bool IsHidden,
    List<CvAttributeDto> Attributes,
    List<CvProjectDto> Projects,
    List<string> PositionTags,
    int? MaxProjectCount,
    int LikesCount,
    bool IsLikedByRequester,
    bool CanLike
);
