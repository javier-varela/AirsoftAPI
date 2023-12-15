using System.Linq.Expressions;

namespace AirsoftAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<List<T>> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
        );
        Task<bool> Add(T entity);
        Task<bool> Remove(int id);
        Task<bool> Remove(T entity);
        Task<bool> Save();
    }
}
