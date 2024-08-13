namespace AuthAPI903.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string NotasAdicionales { get; set; }
        public int? IdUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<AsignacionPaciente> AsignacionPacientes { get; set; }
        public virtual ICollection<PadecimientoPaciente> PadecimientoPacientes { get; set; }

    }
}
