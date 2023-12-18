using AirsoftAPI.Data;
using AirsoftAPI.Models;
using AirsoftAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AirsoftAPI.Repository
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private new readonly ApplicationDbContext _db;

        public ProductoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> Update(Producto producto)
        {
            _db.Productos.Update(producto);
            return await Save();
            
        }
    }
}
