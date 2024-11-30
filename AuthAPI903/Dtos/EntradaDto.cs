namespace AuthAPI903.Dtos
{
    public class EntradaDto
    {
        public string? Contenido { get; set; }
        public DateTime Fecha { get; set; }
        public int? NivelEmocion { get; set; } // Nivel de emoción
        public int? IdEmocion { get; set; }   // ID de la emoción
    }
}
