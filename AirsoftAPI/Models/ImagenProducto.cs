using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftAPI.Models
{
    public class ImagenProducto
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
    }
}
