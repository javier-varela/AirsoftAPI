﻿using AirsoftAPI.Models;
using AirsoftAPI.Models.Dtos;

namespace AirsoftAPI.Repository.IRepository
{
    public interface ICanchaRepository : IRepository<Cancha>
    {
        //Task<bool> Update(UpdateProductoDTO updateProductoDTO);
        Task<Cancha> AddCancha(CrearCanchaDTO crearCanchaDTO);
    }
}
