using AirsoftAPI.Data;
using AirsoftAPI.Models;
using AirsoftAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AirsoftAPI.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private new readonly ApplicationDbContext _db;

        public CategoriaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> Update(Categoria categoria)
        {
            _db.Categorias.Update(categoria);
            return await Save();
            
        }
    }
}
