namespace AuthAPI903.Dtos
{
    public class PacienteDto
    {
        public int IdPaciente { get; set; }
        public string Nombre { get; set; }
        public string ApellidoMaterno { get; set; }
        public string ApellidoPaterno { get; set; }
        public string Telefono { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string Foto { get; set; }
        public string EstadoCivil { get; set; }
        public string Ocupacion { get; set; }
        public List<string>? Domicilios { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string? NotasAdicionales { get; set; }
    }
}