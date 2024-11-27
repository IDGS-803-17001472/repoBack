namespace AuthAPI903.Models
{
    using AuthAPI903.Models;

    public class Cotizacion
    {
        public int Id { get; set; } // Clave primaria de la cotización
        public int ClienteId { get; set; } // Clave foránea al cliente
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Cliente Cliente { get; set; } // Navegación hacia Cliente
    }
}