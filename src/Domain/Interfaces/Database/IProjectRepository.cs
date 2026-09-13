using Ardalis.Specification;
using Domain.Contracts;
using Domain.Entities;
using Domain.ResultPattern.Result;

namespace Domain.Interfaces.Database;

public interface IProjectRepository : IRepositoryBase<Project>
{
    Task<bool> TrySaveWithConcurrencyAsync(
        Project entity,
        int expectedVersion,
        CancellationToken cancellationToken = default
    );

    Task<List<Project>> GetProjectsAsync(
        int userId,
        CancellationToken cancellationToken = default
    );

    Task<Project?> GetProjectAsync(
        int projectId,
        int userId,
        CancellationToken cancellationToken = default
    );

    Task<Result<Project>> TryCreateProjectAsync(
        int userId,
        string title,
        DateTime? startDate,
        DateTime? endDate,
        string descriptionMarkdown,
        IReadOnlyList<string> tags,
        CancellationToken cancellationToken = default
    );

    Task<Result<Project>> TryUpdateProjectAsync(
        int projectId,
        int userId,
        string title,
        DateTime? startDate,
        DateTime? endDate,
        string descriptionMarkdown,
        IReadOnlyList<string> tags,
        int expectedVersion,
        CancellationToken cancellationToken = default
    );

    Task<Result> TryDeleteProjectAsync(
        int projectId,
        int userId,
        int expectedVersion,
        CancellationToken cancellationToken = default
    );
}
