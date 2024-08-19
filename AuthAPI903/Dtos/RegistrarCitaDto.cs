namespace AuthAPI903.Dtos
{
    public class RegistrarCitaDto
    {
        public int IdPaciente { get; set; }
        public DateTime Fecha { get; set; }
        public string Horario { get; set; } // Cambiado a string
        public string? Notas { get; set; }
    }
}