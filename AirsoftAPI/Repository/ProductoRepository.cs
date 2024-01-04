using AirsoftAPI.Data;
using AirsoftAPI.Models;
using AirsoftAPI.Models.Dtos;
using AirsoftAPI.Repository.IRepository;
using AirsoftAPI.Services;
using AirsoftAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using XAct;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AirsoftAPI.Repository
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private new readonly ApplicationDbContext _db;
        private readonly ISupabaseStorageService _supabaseStorageService;


        public ProductoRepository(ApplicationDbContext db, ISupabaseStorageService supabaseStorageService) : base(db)
        {
            _db = db;
            _supabaseStorageService = supabaseStorageService;

        }
        public async Task<Producto> AddProducto(CrearProductoDTO crearProductoDTO)
        {

            Producto producto = new()
            {
                Nombre = crearProductoDTO.Nombre,
                Precio = crearProductoDTO.Precio,
                Descripcion = crearProductoDTO.Descripcion,
                CategoriaId = crearProductoDTO.CategoriaId
            };

            foreach (var imagen in crearProductoDTO.Imagenes.ToList())
            {
                if (imagen == null)
                {
                    throw new Exception("Error guardando la imagen: " + imagen.FileName);
                }
                ImagenProducto imagenProducto = new()
                {
                    Url = await _supabaseStorageService.UploadImageAsync(imagen, "Productos", crearProductoDTO.Nombre)
                };
                producto.Imagenes.Add(imagenProducto);

            }
            await _db.AddAsync(producto);
            if (!await Save())
            {
                throw new Exception("Error creando el producto");
            }

            return producto;
        }
        public async Task<bool> AnyCategoriaAsync(int id)
        {
            return await _db.Categorias.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> Update(Producto producto)
        {
            _db.Productos.Update(producto);

            return await Save();

        }


    }
}

