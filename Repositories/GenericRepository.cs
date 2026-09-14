using Data;
using Entities.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Utilities.PaginationHelper;

namespace Repositories
{
    public class GenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetAll(bool trackChanges = false)
        {
            IQueryable<T> query = _dbSet.Where(e => !e.SoftDelete); // Exclude soft-deleted records
            return trackChanges
                ? query // Track changes
                : query.AsNoTracking(); // Do not track changes
        }

        public async Task<PagedList<T>> GetPaged(int pageNumber, int pageSize, int? dataCount = null, bool trackChanges = false)
        {
            IQueryable<T>? query = null;
            if (trackChanges)
            {
                query = _dbSet.Where(e => !e.SoftDelete); // Exclude soft-deleted records
            }
            else
            {
                query = _dbSet.AsNoTracking().Where(e => !e.SoftDelete); // Exclude soft-deleted records
            }

            return await PagedList<T>.ToPagedListAsync(query, pageNumber, pageSize, dataCount);
        }

        public async Task<T?> GetByIdAsync(int id, bool trackChanges = false)
        {
            IQueryable<T> query = _dbSet.Where(e => e.Id == id && !e.SoftDelete); // Exclude soft-deleted records
            return trackChanges
                ? await query.FirstOrDefaultAsync() // Track changes
                : await query.AsNoTracking().FirstOrDefaultAsync(); // Do not track changes
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool trackChanges = false)
        {
            IQueryable<T> query = _dbSet.Where(e => !e.SoftDelete).Where(predicate); // Exclude soft-deleted records

            return trackChanges
                ? query // Track changes
                : query.AsNoTracking(); // Do not track changes
        }
        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, bool trackChanges = false)
        {
            IQueryable<T> query = _dbSet.Where(predicate); // Exclude soft-deleted records

            return trackChanges
                ? query // Track changes
                : query.AsNoTracking(); // Do not track changes
        }

        public async Task AddAsync(T entity)
        {
            _ = await _dbSet.AddAsync(entity);
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }
        public async Task SoftDeleteAsync(int id)
        {
            T? entity = await _dbSet.FindAsync(id) ?? throw new Exception("Entity not found");

            if (entity != null)
            {
                // Mark as soft-deleted
                entity.SoftDelete = true;

                // Set deletion audit properties
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        public async Task SoftDeleteAsync(T entity)
        {
            if (entity != null)
            {
                // Mark as soft-deleted
                entity.SoftDelete = true;

                // Set deletion audit properties
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        public void Delete(int id)
        {
            T? entity =  _dbSet.Find(id) ?? throw new Exception("Entity not found");

            if (entity != null)
            {
                 _dbSet.Remove(entity);
            }
        }

        public void Delete(T entity)
        {
            if (entity == null)
                throw new Exception("Cannot delete a null entity.");

            _dbSet.Remove(entity);
        }
        public async Task SoftDeleteRangeAsync(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                T? entity = await _dbSet.FindAsync(id);
                entity!.SoftDelete = true;
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        public async Task DeleteRange(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                T? entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                }
            }
        }

        public async Task DeleteRange(IEnumerable<T> entities)
        {
            if (entities != null)
                _dbSet.RemoveRange(entities);
        }
        public async Task SaveChangesAsync()
        {
            _ = await _context.SaveChangesAsync();
        }
    }
}
