using System;

namespace AuthAPI903.Dtos
{
    public class CotizacionesDto
    {
        public int Id { get; set; }
        public required string Cliente { get; set; }
        public required string Descripcion { get; set; }
        public double Precio { get; set; }
        public DateTime Fecha { get; set; }
    }
}