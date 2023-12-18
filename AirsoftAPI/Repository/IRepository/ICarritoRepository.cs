using AirsoftAPI.Models;

namespace AirsoftAPI.Repository.IRepository
{
    public interface ICarritoRepository : IRepository<ProductoCarrito>
    {
        Task<bool> Update(ProductoCarrito productoCarrito);
    }
}
