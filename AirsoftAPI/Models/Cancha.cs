using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models
{
    public class Cancha
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
