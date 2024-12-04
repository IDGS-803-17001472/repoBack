
namespace AuthAPI903.Dtos
{
    public class UserDetailDto2
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Sexo { get; set; }
        public DateTime? FechadeNacimiento { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Titulo { get; set; }
        public string? Ocupacion { get; set; }
        public string? Direccion { get; set; }
        public string? Foto { get; set; }
        public PersonaDto? Persona { get; set; }
        public ProfesionalDto? Profesional { get; set; }
    }
}