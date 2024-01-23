using AirsoftAPI.Data;
using AirsoftAPI.Models;
using AirsoftAPI.Models.Dtos;
using AirsoftAPI.Repository.IRepository;
using AirsoftAPI.Services;
using AirsoftAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using XAct;
using static System.Net.Mime.MediaTypeNames;
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
            if (!crearProductoDTO.Imagenes.IsNullOrEmpty())
            {
                foreach (var imagen in crearProductoDTO.Imagenes.ToList())
                {
                    if (imagen == null)
                    {
                        throw new Exception("Error guardando la imagen: " + imagen.FileName);
                    }
                    ImagenProducto imagenProducto = new()
                    {
                        Url = await _supabaseStorageService.UploadImageAsync(imagen, "Productos", crearProductoDTO.Nombre),
                        FileName = crearProductoDTO.Nombre + "/" + imagen.FileName
                    };

                    producto.Imagenes.Add(imagenProducto);

                }
            }
            await _db.Productos.AddAsync(producto);
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

        public async Task<bool> Update(UpdateProductoDTO updateProductoDTO)
        {
            var producto = await GetFirstOrDefault(p => p.Id == updateProductoDTO.Id, includeProperties: "Imagenes");
            producto.Nombre = updateProductoDTO.Nombre;
            producto.Descripcion = updateProductoDTO.Descripcion;
            producto.Precio = updateProductoDTO.Precio;
            producto.CategoriaId = updateProductoDTO.CategoriaId;
            //Eliminar imagenes de supabase
            List<string> ImagesToRemove = new();

            if (!producto.Imagenes.IsNullOrEmpty())
            {
                if (!updateProductoDTO.Imagenes.IsNullOrEmpty())
                {

                    foreach (var img in producto.Imagenes)
                    {
                        // Verificar si la imagen actual no está presente en updateProductoDTO.NuevasImagenes
                        if (!updateProductoDTO.Imagenes.Any(handleImg => handleImg.Equals(img)))
                        {
                            // Agregar la imagen actual a la lista de imágenes para eliminar
                            ImagesToRemove.Add(img.FileName);
                            producto.Imagenes.Remove(img);

                        }
                    }

                }
                else
                {
                    var imagenes = new List<ImagenProducto>(producto.Imagenes);
                    foreach (var img in imagenes)
                    {
                        // Agregar la imagen actual a la lista de imágenes para eliminar
                        ImagesToRemove.Add(img.FileName);

                        // Remover la imagen de la colección original
                        producto.Imagenes.Remove(img);
                    }
                }
                await _supabaseStorageService.Delete(ImagesToRemove, "Productos");

            }



            //SUBIR NUEVAS IMAGENES

            if (!updateProductoDTO.NewImagenes.IsNullOrEmpty())
            {
                foreach (var imagen in updateProductoDTO.NewImagenes.ToList())
                {
                    if (imagen == null)
                    {
                        throw new Exception("Error guardando la imagen: " + imagen.FileName);
                    }
                    ImagenProducto imagenProducto = new()
                    {
                        Url = await _supabaseStorageService.UploadImageAsync(imagen, "Productos", updateProductoDTO.Nombre),
                        FileName = "Productos/" + updateProductoDTO.Nombre + "/" + imagen.FileName
                    };

                    producto.Imagenes.Add(imagenProducto);

                }
            }


            //Actualizar producto de la base de datos
            _db.Update(producto);

            return await Save();

        }

        public override async Task<bool> Remove(Producto producto)
        {
            List<string> imagesToDelete = new();
            if (!producto.Imagenes.IsNullOrEmpty())
            {
                foreach (var img in producto.Imagenes)
                {
                    imagesToDelete.Add(img.FileName);
                }
                //Corregir, aqui no se estan eliminando las imagenes de supabase
                await _supabaseStorageService.Delete(imagesToDelete, "Productos");
            }
            
            _db.Productos.Remove(producto);

            return await Save();
        }



    }
}

