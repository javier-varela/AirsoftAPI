using AirsoftAPI.Data;
using AirsoftAPI.Models;
using AirsoftAPI.Repository.IRepository;

namespace AirsoftAPI.Repository
{
    public class CompraRepository : Repository<Compra>, ICompraRepository
    {
        private new readonly ApplicationDbContext _db;
        public CompraRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> ComprarProducto(Compra compra)
        {
            var producto = _db.Productos.FirstOrDefault(p=>p.Id == compra.IdProducto);
            var usuario = _db.Usuario.FirstOrDefault(u => u.Id == compra.IdUsuario);
            var total = compra.Cantidad * producto.Precio;
            //validar puntos de usuario
            if (usuario.Puntos<total)
            {
                throw new Exception("No tienes puntos suficientes");
            }
            if (producto.Stock < compra.Cantidad)
            {
                throw new Exception("No hay suficientes existencias del producto");
            }
            usuario.Puntos -= total;
            producto.Stock -= compra.Cantidad;

            await _db.Compras.AddAsync(compra);
           
            if (await Save())
            {
                _db.Productos.Update(producto);
                _db.Usuario.Update(usuario);
               
            }

            return await Save();
        }
    }
}
