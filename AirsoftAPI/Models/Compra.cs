using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftAPI.Models
{
    public class Compra
    {
        [Key]
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        [ForeignKey("IdUsuario")]
        public Usuario Usuario {  get; set; } 
        public int IdProducto { get; set; }
        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}
