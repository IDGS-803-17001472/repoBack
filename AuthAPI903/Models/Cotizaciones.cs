namespace AuthAPI903.Models
{
    public class Cotizacion
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public DateTime Fecha { get; set; }

        public virtual ICollection<Cotizacion> Cotizaciones { get; set; }
    }
}