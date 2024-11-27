namespace AuthAPI903.Dtos
{
    public class PagoDto
    {
        public int Id { get; set; }
        public decimal Cantidad { get; set; }
        public DateTime FechaDePago { get; set; }
        public int SuscripcionId { get; set; }
    }

    public class RegistrarPagoDto
    {
        public decimal Cantidad { get; set; }
        public DateTime FechaDePago { get; set; }
        public int SuscripcionId { get; set; }
    }

    public class ActualizarPagoDto
    {
        public decimal? Cantidad { get; set; }
        public DateTime? FechaDePago { get; set; }
    }
}
