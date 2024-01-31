namespace AirsoftAPI.Models.Dtos
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Puntos { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public List<CompraUserDTO> Compras { get; set; }
    }
    public class CompraUserDTO
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}
