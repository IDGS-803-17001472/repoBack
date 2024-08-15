using System.ComponentModel.DataAnnotations;


namespace AuthAPI903.Dtos
{
    public class RegisterPacienteDto
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public int? profesionalId { get; set; } = null;

        [Required]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [Required]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [Required]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        public DateTime? FechaNacimiento { get; set; }

        [Required]
        public string Sexo { get; set; } = string.Empty;

        public string Foto { get; set; } = string.Empty;

        [Required]
        public string EstadoCivil { get; set; } = string.Empty;

        [Required]
        public string Ocupacion { get; set; } = string.Empty;

        public List<string>? Domicilios { get; set; } = new List<string>();
    }
}