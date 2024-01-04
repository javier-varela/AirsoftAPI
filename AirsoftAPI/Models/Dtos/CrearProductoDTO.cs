using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models.Dtos
{
    public class CrearProductoDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Descripcion es obligatoria")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El Precio es Obligatorio")]
        public decimal Precio { get; set; }
        
        public IFormFileCollection Imagenes { get; set; }
        [Required(ErrorMessage = "La categoria es obligatoria")]
        public int CategoriaId { get; set; }
    }
}
