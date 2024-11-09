namespace AuthAPI903.Models
{
    public class MetodoPago
    {
        public int Id { get; set; }
        public string NumeroCuentaUltimos4 { get; set; }

        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }

        public int TipoDeMetodoId { get; set; }
        public virtual TipoMetodoPago TipoMetodoPago { get; set; }
    }
}
