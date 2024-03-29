﻿using AirsoftAPI.Models;
using AirsoftAPI.Models.Dtos;

namespace AirsoftAPI.Repository.IRepository
{
    public interface IProductoRepository : IRepository<Producto>
    {
        Task<bool> AnyCategoriaAsync(int id);
        Task<bool> Update(UpdateProductoDTO updateProductoDTO);
        Task<Producto> AddProducto(CrearProductoDTO crearProductoDTO);
    }
}
