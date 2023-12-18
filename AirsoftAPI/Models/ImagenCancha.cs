using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models
{
    public class ImagenCancha
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
    }
}
