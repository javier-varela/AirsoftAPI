using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models.Dtos
{
    public class ProductoDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public int Stock { get; set; }

        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public List<ImagenProducto> Imagenes { get; set; }
        public Categoria Categoria { get; set; }
    }
}
