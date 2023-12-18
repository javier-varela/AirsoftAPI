using AirsoftAPI.Models;
using AirsoftAPI.Models.Dtos;

namespace AirsoftAPI.Repository.IRepository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        //Task<bool> Update(Usuario categoria);
        Task<bool> IsUniqueUser(string nombre);
        Task<UsuarioLoginResponseDTO> Login(UsuarioLoginDTO usuariologinDTO);
        Task<Usuario> Registro(UsuarioRegistroDTO usuarioRegistroDTO);
    }
}
