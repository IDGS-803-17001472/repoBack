using System;

namespace AuthAPI903.Dtos
{
    public class ListaPreciosDto
    {
        public int Id { get; set; }
        public required string TipoPlan { get; set; }
        public double PrecioPlan { get; set; }
        public required string Empresa { get; set; }
        public int CantidadLicencias { get; set; }
        public int DuracionContrato { get; set; }
        public double PrecioFinal { get; set; }
    }
}