using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Interfaces.Database;

public interface IAttributeRepository : IRepositoryBase<AttributeDefinition>
{
    Task<bool> TrySaveWithConcurrencyAsync(
        AttributeDefinition entity,
        int expectedVersion,
        CancellationToken cancellationToken = default
    );
}
