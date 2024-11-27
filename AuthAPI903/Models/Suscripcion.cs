namespace AuthAPI903.Models
{
    public class Suscripcion
    {
        public int Id { get; set; }
        public DateTime FechaDeInicio { get; set; }
        public DateTime FechaDeFin { get; set; }
        public string Estado { get; set; }

        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }

        public int PlanId { get; set; }
        public virtual PlanSuscripcion PlanSuscripcion { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
