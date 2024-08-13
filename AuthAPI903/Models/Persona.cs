namespace AuthAPI903.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoMaterno { get; set; }
        public string ApellidoPaterno { get; set; }
        public string Telefono { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string Foto { get; set; }
        public string EstadoCivil { get; set; }
        public string Ocupacion { get; set; }

        public virtual ICollection<Domicilio> Domicilios { get; set; }
        public virtual Usuario Usuario { get; set; }

    }
}
