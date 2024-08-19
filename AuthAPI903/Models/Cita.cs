namespace AuthAPI903.Models
{
    public class Cita
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string Estatus { get; set; }
        public string? Notas { get; set; }
        public TimeSpan? Horario { get; set; }
        public int? IdAsignacion { get; set; }

        public virtual AsignacionPaciente AsignacionPaciente { get; set; }
    }

}
