using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftAPI.Models.Dtos
{
    public class CompraDTO
    {
        public int IdUsuario { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }


    
}
