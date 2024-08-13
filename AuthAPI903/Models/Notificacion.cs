namespace AuthAPI903.Models
{
    public class Notificacion
    {
        public int Id { get; set; }
        public int? IdUsuario { get; set; }
        public string Tipo { get; set; }
        public string Estatus { get; set; }
        public string Contenido { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
