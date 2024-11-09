namespace AuthAPI903.Models
{
    public class PlanSuscripcion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public int DuracionMeses { get; set; } 

        public virtual ICollection<Suscripcion> Suscripciones { get; set; }
    }
}
