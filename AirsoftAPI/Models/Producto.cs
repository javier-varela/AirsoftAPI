﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftAPI.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
      
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public List<ImagenProducto> Imagenes { get; set; } = new List<ImagenProducto>();

        [ForeignKey("CategoriaId")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }



    }
}
