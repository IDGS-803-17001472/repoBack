namespace AuthAPI903.Dtos
{
    public class CitaPacienteDto
    {
        public int Id { get; set; }
        public string Title { get; set; } // Nota o título de la cita
        public DateTime Date { get; set; } // Fecha de la cita
        public TimeSpan Time { get; set; } // Hora de la cita
        public string Status { get; set; } // Estatus de la cita
        public ProfesionalCitaDto Profesional { get; set; } // Detalles del profesional asociado
    }

}
