namespace AuthAPI903.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public decimal Cantidad { get; set; }
        public DateTime FechaDePago { get; set; }

        public int SuscripcionId { get; set; }
        public virtual Suscripcion Suscripcion { get; set; }
    }
}
