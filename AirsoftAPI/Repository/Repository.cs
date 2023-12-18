using AirsoftAPI.Data;
using AirsoftAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AirsoftAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext context)
        {
            _db = context;
            dbSet = _db.Set<T>();
        }
        public async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate);
           
        }

        public async Task<T> Get(int id)
        {
            return await dbSet.FindAsync(id);
        }

        
        public async Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            if (orderBy != null)
            {
                _ = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }


        public Task<bool> Remove(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Remove(T entity)
        {
            dbSet.Remove(entity);
            return await Save();
        }

        public async Task<bool> RemoveAll(List<T> list)
        {
            dbSet.RemoveRange(list);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync()>=0;
        }
    }
}
