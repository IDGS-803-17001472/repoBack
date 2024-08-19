namespace AuthAPI903.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string? NotasAdicionales { get; set; }
        public int? IdPersona { get; set; }
        public Boolean Estatus { get; set; } = true;

        public virtual Persona Persona { get; set; }
        public virtual ICollection<AsignacionPaciente>? AsignacionPacientes { get; set; }
        public virtual ICollection<PadecimientoPaciente>? PadecimientoPacientes { get; set; }
        public virtual ICollection<Entrada>? Entradas { get; set; }

    }
}
