using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models.Dtos
{
    public class UpdateProductoDTO
    {
        [Required(ErrorMessage = "El ID es obligatorio")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Descripcion es obligatoria")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El Precio es Obligatorio")]
        public decimal Precio { get; set; }
        public IFormFileCollection NewImagenes { get; set; }
        public List<ImagenProducto> Imagenes { get; set; }
        public int CategoriaId { get; set; }
    }
}
