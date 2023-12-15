using AirsoftAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AirsoftAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nombre = "Armas" },
                new Categoria { Id = 2, Nombre = "Cascos" },
                new Categoria { Id = 3, Nombre = "Chalecos" }
            );

            modelBuilder.Entity<Producto>().HasData(
                new Producto { Id = 1, Nombre = "Fusil de Asalto", Descripcion = "Réplica de fusil de asalto con mira telescópica", Precio = 599.99m, Imagen = "fusil.jpg", CategoriaId = 1 },
                new Producto { Id = 2, Nombre = "Casco Táctico", Descripcion = "Casco resistente con visor y protección balística", Precio = 149.99m, Imagen = "casco.jpg", CategoriaId = 2 },
                new Producto { Id = 3, Nombre = "Chaleco Táctico", Descripcion = "Chaleco con múltiples compartimentos y sistema MOLLE", Precio = 89.99m, Imagen = "chaleco.jpg", CategoriaId = 3 },
                new Producto { Id = 4, Nombre = "Pistola de Airsoft", Descripcion = "Réplica de pistola semiautomática para airsoft", Precio = 129.99m, Imagen = "pistola.jpg", CategoriaId = 1 },
                new Producto { Id = 5, Nombre = "Granada de Humo", Descripcion = "Granada de humo para estrategias tácticas", Precio = 19.99m, Imagen = "granada.jpg", CategoriaId = 1 },
                new Producto { Id = 6, Nombre = "Máscara Facial", Descripcion = "Máscara protectora para el rostro con diseño intimidante", Precio = 49.99m, Imagen = "mascara.jpg", CategoriaId = 2 },
                new Producto { Id = 7, Nombre = "Chaqueta Táctica", Descripcion = "Chaqueta resistente con bolsillos y paneles de velcro", Precio = 79.99m, Imagen = "chaqueta.jpg", CategoriaId = 3 },
                new Producto { Id = 8, Nombre = "Munición BB", Descripcion = "Munición de plástico para pistolas y rifles de airsoft", Precio = 9.99m, Imagen = "bb.jpg", CategoriaId = 1 },
                new Producto { Id = 9, Nombre = "Lanzagranadas", Descripcion = "Lanzagranadas para acoplar a rifles de airsoft", Precio = 199.99m, Imagen = "lanzagranadas.jpg", CategoriaId = 1 }
            );
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

    }
}
