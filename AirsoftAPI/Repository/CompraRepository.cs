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

    }
}
