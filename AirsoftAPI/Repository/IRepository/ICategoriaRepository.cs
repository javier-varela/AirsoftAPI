using AirsoftAPI.Models;

namespace AirsoftAPI.Repository.IRepository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<bool> Update(Categoria categoria);

        Task<bool> Exists(string nombre);

        Task<bool> Exists(int id);
    }
}
