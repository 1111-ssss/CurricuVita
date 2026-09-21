using Domain.Entities;
using Domain.Interfaces.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class AttributeRepository : BaseRepository<AttributeDefinition>, IAttributeRepository
{
    private readonly AppDbContext _context;

    public AttributeRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> TrySaveWithConcurrencyAsync(
        AttributeDefinition entity,
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

    public async Task<int> DeleteAttributesBulkAsync(
        ICollection<int> ids,
        CancellationToken cancellationToken = default
    )
    {
        if (ids is null || ids.Count == 0)
        {
            return 0;
        }

        return await _context.AttributeDefinitions
            .Where(a => ids.Contains(a.Id) && a.Category != "Me")
            .ExecuteDeleteAsync(cancellationToken);
    }
}
