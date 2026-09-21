using Domain.Interfaces.Database;
using Ardalis.Specification.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class BaseRepository<TEntity> : RepositoryBase<TEntity>
    where TEntity : class, IEntity
{
    public BaseRepository(AppDbContext context) : base(context) { }
}