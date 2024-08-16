using System.ComponentModel.DataAnnotations;


namespace AuthAPI903.Dtos
{
    public class PersonaPacienteDto
    {
        public int IdPaciente { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? Telefono { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Sexo { get; set; }
        public string? Email { get; set; }
    }

}