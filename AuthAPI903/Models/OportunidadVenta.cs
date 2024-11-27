namespace AuthAPI903.Models
{
    public class OportunidadVenta
    {
        public int Id { get; set; }
        public required string NombreCliente { get; set; }
        public required string Descripcion { get; set; }
        public double ValorEstimado { get; set; }
        public DateTime FechaCierre { get; set; }
        public required string Estado { get; set; }
    }
}