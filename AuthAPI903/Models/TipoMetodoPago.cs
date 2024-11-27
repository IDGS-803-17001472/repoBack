namespace AuthAPI903.Models
{
    public class TipoMetodoPago
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<MetodoPago> MetodosPago { get; set; }
    }
}
