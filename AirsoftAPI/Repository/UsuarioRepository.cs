using AirsoftAPI.Data;
using AirsoftAPI.Models;
using AirsoftAPI.Models.Dtos;
using AirsoftAPI.Repository.IRepository;
using AirsoftAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace AirsoftAPI.Repository
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private new readonly ApplicationDbContext _db;
        private readonly string secretPassword;

        public UsuarioRepository(ApplicationDbContext db, IConfiguration config) : base(db)
        {
            _db = db;
            secretPassword = config.GetValue<string>("ApiSettings:Secret");
        }
        public async Task<UsuarioLoginResponseDTO> Login(UsuarioLoginDTO usuarioLoginDTO)
        {
            var encryptedPassword = Getmd5(usuarioLoginDTO.Password);
            var usuario = await GetFirstOrDefault(
                u => u.Nombre.ToLower() == usuarioLoginDTO.Nombre.ToLower()
                && u.Password == encryptedPassword);

            if (usuario == null)
            {
                return new UsuarioLoginResponseDTO()
                {
                    Token = "",
                    Usuario = null
                };
            }
            var handleToken = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretPassword));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, usuario.Nombre.ToString()),
                    new(ClaimTypes.Role, usuario.Rol),
                    new(ClaimTypes.NameIdentifier, usuario.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(key, SecurityAlgorithms.HmacSha256)
            };
            var token = handleToken.CreateToken(tokenDescriptor);
            UsuarioLoginResponseDTO usuarioLoginResponseDTO = new()
            {
                Token = handleToken.WriteToken(token),
                Usuario = usuario
            };
            return usuarioLoginResponseDTO;
        }

        public async Task<Usuario> Registro(UsuarioRegistroDTO usuarioRegistroDTO)
        {
            var encryptedPassword = Getmd5(usuarioRegistroDTO.Password);

            Usuario usuario = new()
            {
                Nombre = usuarioRegistroDTO.Nombre,
                Password = encryptedPassword,
                Rol = UserRoles.Client,
                Puntos = 10000
            };
            await _db.Usuario.AddAsync(usuario);
            await Save();
            usuario.Password = encryptedPassword;
            return usuario;
        }

        public static string Getmd5(string valor)
        {
            MD5CryptoServiceProvider x = new();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }

        public async Task<bool> IsUniqueUser(string nombre)
        {
            var usuariodb = await _db.Usuario.FirstOrDefaultAsync(u => u.Nombre == nombre);
            if (usuariodb == null)
            {
                return true;
            }
            return false;
        }
    }
}
