using AirsoftAPI.Models;

namespace AirsoftAPI.Repository.IRepository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<bool> Update(Categoria categoria);
    }
}
