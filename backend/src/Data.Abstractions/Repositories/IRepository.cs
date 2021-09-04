using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BoringSoftware.Finances.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> FindOneByPredicatingAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> FindManyByPredicatingAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> AddOneAsync(TEntity entity);

        Task<List<TEntity>> AddManyAsync(IEnumerable<TEntity> entities);
    }
}
