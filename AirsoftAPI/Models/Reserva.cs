using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models
{
    public class Reserva
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int MyProperty { get; set; }
    }
}
