using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models.Dtos
{
    public class CrearReservaDTO
    {

        public int CanchaId { get; set; }

        public int UsuarioId { get; set; }

        public int Duracion { get; set; }
    }
}
