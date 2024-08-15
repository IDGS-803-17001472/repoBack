namespace AuthAPI903.Models
{
    public class Profesional
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int? IdUsuario { get; set; }
        public Boolean Estatus { get; set; } = true;

        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<AsignacionPaciente> AsignacionPacientes { get; set; }
        public virtual ICollection<DocumentoProfesional> DocumentoProfesionales { get; set; }
    }
}
