using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Puntos { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public List<Compra> Compras { get; set; } = new();

    }
}
