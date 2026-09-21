using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Interfaces.Database;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<bool> TrySaveWithConcurrencyAsync(
        User entity,
        int expectedVersion,
        CancellationToken cancellationToken = default
    );

    Task<int> CountUsersInRoleAsync(
        string roleName,
        CancellationToken cancellationToken = default
    );
}
