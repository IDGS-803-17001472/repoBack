namespace AuthAPI903.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string IdAppUser { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public string TipoUsuario { get; set; }
        public int? IdPersona { get; set; }
        public int? IdRol { get; set; }
        public string IdentificadorUnico { get; set; }

        public virtual Persona Persona { get; set; }
        public virtual Rol Rol { get; set; }
        public virtual AppUser AuthUser { get; set; }
        public virtual Profesional? Profesional { get; set; }
        public virtual ICollection<Paciente> Pacientes { get; set; }
        public virtual ICollection<Profesional> Profesionales { get; set; }
        public virtual ICollection<Notificacion> Notificaciones { get; set; }

    }
}
