namespace Domain.Contracts.CvContracts;

public record CvProjectDto(
    int Id,
    string Title,
    DateTime? StartDate,
    DateTime? EndDate,
    string DescriptionMarkdown,
    List<string> Tags,
    int Version
);