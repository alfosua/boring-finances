using Microsoft.EntityFrameworkCore;

namespace BoringFinances.Operations.WebApi.Utilities;

public static class DbContextExtensions
{
    public static DbContextEntityPipes<TEntity> Piping<TEntity>(this DbContext dbContext, TEntity entity)
        where TEntity : class
    {
        return new(dbContext, entity);
    }
}

public class DbContextEntityPipes<TEntity>
    where TEntity : class
{
    private readonly DbContext _dbContext;
    private readonly TEntity _entity;

    public DbContextEntityPipes(DbContext dbContext, TEntity entity)
    {
        _dbContext = dbContext;
        _entity = entity;
    }
    public async Task<TEntity> AddAndSaveChangesAsync()
    {
        _dbContext.Set<TEntity>().Add(_entity);
        await _dbContext.SaveChangesAsync();
        return _entity;
    }

    public async Task<TEntity> UpdateAndSaveChangesAsync()
    {
        _dbContext.Set<TEntity>().Update(_entity);
        await _dbContext.SaveChangesAsync();
        return _entity;
    }

    public TEntity AddRangeFromInto<TEntityToAdd>(Func<TEntity, IEnumerable<TEntityToAdd>> selector)
        where TEntityToAdd : class
    {
        var entities = selector.Invoke(_entity);
        _dbContext.Set<TEntityToAdd>().AddRange(entities);
        return _entity;
    }
}
