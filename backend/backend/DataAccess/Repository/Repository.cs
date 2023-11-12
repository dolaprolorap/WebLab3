using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace backend.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        public Repository(AppDbContext db)
        {
            _dbSet = db.Set<T>();
        } 
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public IEnumerable<T> ReadAll()
        {
            return _dbSet.ToList();
        }

        public IQueryable<T> ReadWhere(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbSet;
            return query.Where(filter);
        }

        public T? ReadFirst(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbSet;
            return query.Where(filter).FirstOrDefault();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
