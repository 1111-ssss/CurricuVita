namespace Domain.Contracts.UserContracts;

public record ProjectDto(
    int Id,
    string Title,
    DateTime? StartDate,
    DateTime? EndDate,
    string DescriptionMarkdown,
    List<string> Tags,
    int Version
);
