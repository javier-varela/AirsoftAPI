using AirsoftAPI.Models;

namespace AirsoftAPI.Repository.IRepository
{
    public interface IProductoRepository : IRepository<Producto>
    {
        Task<bool> Update(Producto producto);

        Task<bool> Exists(string nombre);

        Task<bool> Exists(int id);
    }
}
