using Domain.Entities;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Profile;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> TrySaveWithConcurrencyAsync(
        Project entity,
        int expectedVersion,
        CancellationToken cancellationToken = default
    )
    {
        _context.Entry(entity).Property(e => e.Version).OriginalValue = expectedVersion;
        entity.Version = expectedVersion + 1;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    public Task<List<Project>> GetProjectsAsync(int userId, CancellationToken cancellationToken = default) =>
        ListAsync(new ProjectsByUserSpec(userId), cancellationToken);

    public Task<Project?> GetProjectAsync(int projectId, int userId, CancellationToken cancellationToken = default) =>
        SingleOrDefaultAsync(new ProjectByIdAndUserSpec(projectId, userId), cancellationToken);

    public async Task<Result<Project>> TryCreateProjectAsync(
        int userId,
        string title,
        DateTime? startDate,
        DateTime? endDate,
        string descriptionMarkdown,
        IReadOnlyList<string> tags,
        CancellationToken cancellationToken = default
    )
    {
        if (!await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken))
        {
            return Result<Project>.Failure(Errors.UserNotFound);
        }
        if (startDate.HasValue && endDate.HasValue && endDate < startDate)
        {
            return Result<Project>.Failure(Errors.ValidationFailed);
        }

        var project = new Project
        {
            UserId = userId,
            Title = title.Trim(),
            Location = string.Empty,
            StartDate = startDate,
            EndDate = endDate,
            DescriptionMarkdown = descriptionMarkdown,
            Version = 1
        };

        _context.Projects.Add(project);
        await SyncProjectTagsAsync(project, tags, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Project>.Success(
            (await GetProjectAsync(project.Id, userId, cancellationToken))!
        );
    }

    public async Task<Result<Project>> TryUpdateProjectAsync(
        int projectId,
        int userId,
        string title,
        DateTime? startDate,
        DateTime? endDate,
        string descriptionMarkdown,
        IReadOnlyList<string> tags,
        int expectedVersion,
        CancellationToken cancellationToken = default
    )
    {
        var project = await SingleOrDefaultAsync(new ProjectByIdAndUserSpec(projectId, userId), cancellationToken);
        if (project is null)
        {
            return Result<Project>.Failure(Errors.NotFound);
        }
        if (project.Version != expectedVersion)
        {
            return Result<Project>.Failure(Errors.ConcurrencyConflict);
        }
        if (startDate.HasValue && endDate.HasValue && endDate < startDate)
        {
            return Result<Project>.Failure(Errors.ValidationFailed);
        }

        project.Title = title.Trim();
        project.StartDate = startDate;
        project.EndDate = endDate;
        project.DescriptionMarkdown = descriptionMarkdown;
        await SyncProjectTagsAsync(project, tags, cancellationToken);

        if (!await TrySaveWithConcurrencyAsync(project, expectedVersion, cancellationToken))
        {
            return Result<Project>.Failure(Errors.ConcurrencyConflict);
        }

        return Result<Project>.Success(
            (await GetProjectAsync(projectId, userId, cancellationToken))!
        );
    }

    public async Task<Result> TryDeleteProjectAsync(
        int projectId,
        int userId,
        int expectedVersion,
        CancellationToken cancellationToken = default
    )
    {
        var project = await SingleOrDefaultAsync(new ProjectByIdAndUserSpec(projectId, userId), cancellationToken);
        if (project is null)
        {
            return Result.Failure(Errors.NotFound);
        }
        if (project.Version != expectedVersion)
        {
            return Result.Failure(Errors.ConcurrencyConflict);
        }

        _context.Entry(project).Property(p => p.Version).OriginalValue = expectedVersion;
        _context.Projects.Remove(project);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Failure(Errors.ConcurrencyConflict);
        }

        return Result.Success();
    }

    private async Task SyncProjectTagsAsync(
        Project project,
        IReadOnlyList<string> tags,
        CancellationToken cancellationToken
    )
    {
        var normalized = tags
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(20)
            .ToList();

        var existingTags = await _context.Tags
            .Where(t => normalized.Contains(t.Name))
            .ToListAsync(cancellationToken);

        foreach (var name in normalized)
        {
            var tag = existingTags.FirstOrDefault(t => string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase));
            if (tag is null)
            {
                tag = new Tag { Name = name };
                _context.Tags.Add(tag);
                existingTags.Add(tag);
            }
            if (!project.Tags.Any(pt => pt.Tag is not null && string.Equals(pt.Tag.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                project.Tags.Add(new ProjectTag { Project = project, Tag = tag });
            }
        }

        var stale = project.Tags
            .Where(pt => pt.Tag is not null && !normalized.Contains(pt.Tag.Name, StringComparer.OrdinalIgnoreCase))
            .ToList();

        foreach (var link in stale)
        {
            project.Tags.Remove(link);
        }
    }
}
