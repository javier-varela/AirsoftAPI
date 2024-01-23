using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models.Dtos
{
    public class CrearCompraDTO
    {
        public int IdUsuario { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
    }
}
