namespace AuthAPI903.Models
{
    public class AsignacionPaciente
    {
        public int Id { get; set; }
        public int? IdProfesional { get; set; }
        public int? IdPaciente { get; set; }

        public virtual Paciente Paciente { get; set; }
        public virtual Profesional Profesional { get; set; }

    }
}
