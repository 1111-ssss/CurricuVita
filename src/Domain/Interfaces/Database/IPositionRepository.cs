using Ardalis.Specification;
using Domain.Entities;
using Domain.Specifications.Positions;

namespace Domain.Interfaces.Database;

public interface IPositionRepository : IRepositoryBase<Position>
{
    Task<bool> TrySaveWithConcurrencyAsync(
        Position entity,
        int expectedVersion,
        CancellationToken cancellationToken = default
    );

    Task<List<PositionListProjection>> SearchPositionsAsync(
        string? titlePrefix,
        bool? onlyPublic,
        int skip,
        int take,
        string? searchText = null,
        string? company = null,
        string? level = null,
        CancellationToken cancellationToken = default
    );

    Task<List<CV>> SearchPositionCvsAsync(
        int positionId,
        string? searchText = null,
        CancellationToken cancellationToken = default
    );

    Task<int> DeletePositionsBulkAsync(
        ICollection<int> ids,
        CancellationToken cancellationToken = default
    );
}
