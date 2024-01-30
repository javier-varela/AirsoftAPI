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
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<Compra, CrearCompraDTO>().ReverseMap();
            CreateMap<Compra, CompraDTO>().ReverseMap();
            CreateMap<Cancha, CrearCanchaDTO>().ReverseMap();
            CreateMap<Cancha, CanchaDTO>().ReverseMap();

        }
    }
}
