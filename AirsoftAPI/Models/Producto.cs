using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftAPI.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public decimal Precio { get; set; }
        [Required]
        public string Imagen { get; set; }
        [ForeignKey("categoriaId")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

    }
}
