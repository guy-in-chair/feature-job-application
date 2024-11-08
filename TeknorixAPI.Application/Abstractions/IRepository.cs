using System.Linq.Expressions;

namespace TeknorixAPI.Application.Abstractions
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        bool Exists(Expression<Func<TEntity, bool>> predicate);
        ICollection<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> AddAsync(TEntity entity);
        TEntity Add(TEntity entity);
        Task AddRangeAsync(ICollection<TEntity> entities);
        Task<TEntity> UpdateAsync(TEntity entity);
        TEntity Update(TEntity entity);
        Task Remove(TEntity entity);
        Task RemoveRange(ICollection<TEntity> entities);
        int Count(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
    }


}
