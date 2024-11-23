namespace AuthAPI903.Models
{
    using AuthAPI903.Models;

    public class Cotizacion
    {
        public int Id { get; set; } // Clave primaria de la cotizaci�n
        public int ClienteId { get; set; } // Clave for�nea al cliente
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Cliente Cliente { get; set; } // Navegaci�n hacia Cliente
    }
}