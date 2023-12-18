using AirsoftAPI.Data;
using AirsoftAPI.Models;
using AirsoftAPI.Repository.IRepository;

namespace AirsoftAPI.Repository
{
    public class CarritoRepository : Repository<ProductoCarrito>, ICarritoRepository
    {
        private new readonly ApplicationDbContext _db;

        public CarritoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> Update(ProductoCarrito productoCarrito)
        {
            _db.ProductosCarrito.Update(productoCarrito);
            return await Save();

        }
    }
}
