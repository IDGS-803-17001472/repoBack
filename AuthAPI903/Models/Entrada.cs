namespace AuthAPI903.Models
{
    public class Entrada
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Contenido { get; set; }
        public int? IdPaciente { get; set; }

        public virtual Paciente Paciente { get; set; }
        public virtual ICollection<Medicion> Mediciones { get; set; }

    }
}
