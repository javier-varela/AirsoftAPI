using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models.Dtos
{
    public class CrearCanchaDTO
    {
        [Required]
        public string Nombre { get; set; }
        public IFormFileCollection Imagenes { get; set; }
        [Required]
        public decimal PrecioHora { get; set; }
        [Required]
        public int JugadoresMinimos { get; set; }
        [Required]
        public decimal Area { get; set; }
    }
}
