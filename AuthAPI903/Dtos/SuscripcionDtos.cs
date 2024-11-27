namespace AuthAPI903.Dtos
{
    public class SuscripcionDto
    {
        public int Id { get; set; }
        public DateTime FechaDeInicio { get; set; }
        public DateTime FechaDeFin { get; set; }
        public string Estado { get; set; }

        public int ClienteId { get; set; }
        public int PlanId { get; set; }
        public List<int> PagoIds { get; set; } // IDs de pagos asociados
    }

    public class RegistrarSuscripcionDto
    {
        public DateTime FechaDeInicio { get; set; }
        public DateTime FechaDeFin { get; set; }
        public string Estado { get; set; }

        public int ClienteId { get; set; }
        public int PlanId { get; set; }
    }

    public class ActualizarSuscripcionDto
    {
        public DateTime FechaDeInicio { get; set; }
        public DateTime FechaDeFin { get; set; }
        public string Estado { get; set; }

        public int ClienteId { get; set; }
        public int PlanId { get; set; }
    }
}
