using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models.Dtos
{
    public class UsuarioLoginDTO
    {
        [Required(ErrorMessage = "Los datos son incorrectos")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Los datos son incorrectos")]
        public string Password { get; set; }
    }
}
