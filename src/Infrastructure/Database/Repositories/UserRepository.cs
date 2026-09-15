using Domain.Entities;
using Domain.Interfaces.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<int> CountUsersInRoleAsync(
        string roleName,
        CancellationToken cancellationToken = default
    )
    {
        var roleId = await _context.Roles
            .Where(r => r.Name == roleName)
            .Select(r => r.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (roleId == default)
        {
            return 0;
        }

        return await _context.UserRoles.CountAsync(
            ur => ur.RoleId == roleId, cancellationToken);
    }

    public async Task<bool> TrySaveWithConcurrencyAsync(
        User entity,
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
}
