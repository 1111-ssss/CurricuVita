using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Interfaces.Database;

public interface IPositionRepository : IRepositoryBase<Position>
{
    Task<bool> TrySaveWithConcurrencyAsync(
        Position entity,
        int expectedVersion,
        CancellationToken cancellationToken = default
    );
}
