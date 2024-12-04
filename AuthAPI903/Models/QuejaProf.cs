namespace AuthAPI903.Models
{
    public class QuejaProf
    {
        public int? Id { get; set; }
        public int? IdUsuarioSolicita { get; set; }
        public int? IdUsuarioNecesita { get; set; }
        public int Estatus { get; set; }
        public int Tipo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
