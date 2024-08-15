namespace AuthAPI903.Dtos
{
    public class EmocionSemanaDto
    {
        public int Anio { get; set; }
        public int Semana { get; set; }
        public List<EmocionDto> Emociones { get; set; }
    }

}
