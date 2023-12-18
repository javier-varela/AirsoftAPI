using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models
{
    public class Cancha
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<Reserva> Reservas { get; set; }
        public List<ImagenCancha> Imagenes { get; set; }
        public decimal PrecioHora { get; set; }
        public int JugadoresMinimos { get; set; }
        public decimal Area { get; set; }
    }
}
