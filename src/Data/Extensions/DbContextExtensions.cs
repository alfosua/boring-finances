using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BoringSoftware.Finances.Data
{
    public static class DbContextExtensions
    {
        public static T Modify<T>(this DbSet<T> dbSet, T entity, Func<T, T> entityDecorator) where T : class
        {
            var entry = dbSet.Attach(entity);

            entityDecorator.Invoke(entity);

            foreach (var property in entry.Properties)
            {
                var original = property.OriginalValue;
                var current = property.CurrentValue;

                if (ReferenceEquals(original, current))
                {
                    continue;
                }

                bool hasBeenModified = (original is null && current is not null) || !original.Equals(current);

                property.IsModified = hasBeenModified;
            }

            return entity;
        }

        public static async Task<TEntity> PassAndSaveChangesAsync<TDbContext, TEntity>(this TDbContext dbContext, TEntity entity)
            where TDbContext : DbContext
            where TEntity : class
        {
            await dbContext.SaveChangesAsync();

            return entity;
        }

        public static async Task<TEntity> ModifyAndSaveChangesAsync<TDbContext, TEntity>(this TDbContext dbContext, TEntity entity, Func<TEntity, TEntity> entityDecorator)
            where TDbContext : DbContext
            where TEntity : class
        {
            var entityWithModifications = dbContext.Set<TEntity>().Modify(entity, entityDecorator);

            await dbContext.SaveChangesAsync();

            return entityWithModifications;
        }

        public static async Task<TEntity> AddAndSaveChangesAsync<TDbContext, TEntity>(this TDbContext dbContext, TEntity entity)
            where TDbContext : DbContext
            where TEntity : class
        {
            dbContext.Set<TEntity>().Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public static async Task<TEntity> DeleteAndSaveChangesAsync<TDbContext, TEntity>(this TDbContext dbContext, TEntity entity, bool attach = false)
            where TDbContext : DbContext
            where TEntity : class
        {
            if (attach)
            {
                dbContext.Attach(entity);
            }

            dbContext.Set<TEntity>().Remove(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
