using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftAPI.Models
{
    public class Reserva
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CanchaId { get; set; }
        [ForeignKey("CanchaId")]
        public Cancha Cancha { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
        public int Duracion { get; set; }
    }
}
