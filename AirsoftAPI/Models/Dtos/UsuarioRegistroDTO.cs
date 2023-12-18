using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models.Dtos
{
    public class UsuarioRegistroDTO
    {
        [Required(ErrorMessage ="El usuario es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "la contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
