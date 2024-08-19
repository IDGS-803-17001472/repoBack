namespace AuthAPI903.Dtos
{
    public class UpdatePacienteDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public string Sexo { get; set; } = string.Empty;
        public string EstadoCivil { get; set; } = string.Empty;
        public string Ocupacion { get; set; } = string.Empty;
    }
}
