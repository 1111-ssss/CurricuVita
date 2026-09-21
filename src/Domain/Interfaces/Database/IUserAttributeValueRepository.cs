using Ardalis.Specification;
using Domain.Contracts.AttributeContracts;
using Domain.Entities;
using Domain.ResultPattern.Result;

namespace Domain.Interfaces.Database;

public interface IUserAttributeValueRepository : IRepositoryBase<UserAttributeValue>
{
    Task<Result<List<UserAttributeValue>>> TrySaveAttributeValuesAsync(
        int userId,
        IReadOnlyList<AttributeValueInput> items,
        CancellationToken cancellationToken = default
    );

    Task<Result> RemoveAttributeValueAsync(
        int userId,
        int valueId,
        CancellationToken cancellationToken = default
    );
}
