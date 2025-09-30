using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BalzorAppVlan.Repository.BaseRepository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync(bool tracking = false);
        Task<T?> GetByIdAsync(Guid id, bool tracking = false);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, bool tracking = false);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool tracking = false);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

        Task<int> SaveChangesAsync();
    }

    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<List<T>> GetAllAsync(bool tracking = false)
        {
            var query = _dbSet.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking().AsSplitQuery();
            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, bool tracking = false)
        {
            var query = _dbSet.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, bool tracking = false)
        {
            var query = _dbSet.Where(predicate);
            if (!tracking)
                query = query.AsNoTracking().AsSplitQuery();
            return await query.ToListAsync();
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().AnyAsync(predicate);
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool tracking = false)
        {
            var query = _dbSet.Where(predicate);
            if (!tracking)
                query = query.AsNoTracking().AsSplitQuery();
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null
                ? await _dbSet.CountAsync()
                : await _dbSet.CountAsync(predicate);
        }

        // ================= CRUD =================

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
