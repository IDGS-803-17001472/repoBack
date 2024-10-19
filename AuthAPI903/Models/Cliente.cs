namespace AuthAPI903.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string CorreoElectronico { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaDeRegistro { get; set; }

        public virtual ICollection<Suscripcion> Suscripciones { get; set; }
        public virtual ICollection<MetodoPago> MetodosPago { get; set; }
    }
}
