namespace AuthAPI903.Dtos
{
    public class PlanSuscripcionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public int DuracionMeses { get; set; }
    }

    public class RegistrarPlanSuscripcionDto
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public int DuracionMeses { get; set; }
    }

    public class ActualizarPlanSuscripcionDto
    {
        public string Nombre { get; set; }
        public decimal? Precio { get; set; }
        public string Descripcion { get; set; }
        public int? DuracionMeses { get; set; }
    }

}
