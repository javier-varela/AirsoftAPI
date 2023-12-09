using System.ComponentModel.DataAnnotations;

namespace AirsoftAPI.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }


    }
}
