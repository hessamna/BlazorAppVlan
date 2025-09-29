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
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }

    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
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

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;

            // جلوگیری از تغییر فیلدهای سیستمی
            entry.Property(nameof(BaseEntity.CreatedDate)).IsModified = false;
            entry.Property(nameof(BaseEntity.CreatedBy)).IsModified = false;
            entry.Property(nameof(BaseEntity.CreatorIp)).IsModified = false;
            entry.Property(nameof(BaseEntity.CreatorMachine)).IsModified = false;

            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted; // 👈 بدون Attach اضافه
            await _context.SaveChangesAsync();
        }
    }
}
