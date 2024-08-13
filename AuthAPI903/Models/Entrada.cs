namespace AuthAPI903.Models
{
    public class Entrada
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string Contenido { get; set; }
        public int? IdUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }

    }
}
