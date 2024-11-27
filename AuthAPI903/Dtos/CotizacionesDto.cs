namespace AuthAPI903.Dtos
{
    public class CotizacionesDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; } // Solo se utiliza el Id del cliente
        public required string Descripcion { get; set; }
        public double Precio { get; set; }
        public DateTime Fecha { get; set; }
    }
}
