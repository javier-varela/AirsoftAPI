using AirsoftAPI.Models;
using AirsoftAPI.Models.Dtos;
using AutoMapper;

namespace AirsoftAPI.AirsoftMapper
{
    public class AirsoftMapper : Profile
    {
        public AirsoftMapper()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<Categoria, CrearCategoriaDTO>().ReverseMap();
            CreateMap<Producto, ProductoDTO>().ReverseMap();
            CreateMap<Producto, CrearProductoDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<ProductoCarrito, ProductoCarritoDTO>().ReverseMap();

        }
    }
}
