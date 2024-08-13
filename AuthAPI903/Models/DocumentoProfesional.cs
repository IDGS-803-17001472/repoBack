namespace AuthAPI903.Models
{
    public class DocumentoProfesional
    {
        public int Id { get; set; }
        public string Estudios { get; set; }
        public string Url { get; set; }
        public int? IdProfesional { get; set; }

        public virtual Profesional Profesional { get; set; }
    }
}
