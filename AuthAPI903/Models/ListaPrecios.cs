namespace AuthAPI903.Models
{
    public class ListaPrecios
    {
        public int Id { get; set; }
        public required string TipoPlan { get; set; }
        public double PrecioPlan { get; set; }
        public required string Empresa { get; set; }
        public int CantidadLicencias { get; set; }
        public int DuracionContrato { get; set; }
        public double PrecioFinal { get; set; }

        public virtual ICollection<ListaPrecios> ListaDePrecios { get; set; }
    }
}