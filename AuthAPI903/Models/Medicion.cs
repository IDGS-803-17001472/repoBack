namespace AuthAPI903.Models
{
    public class Medicion
    {
        public int Id { get; set; }
        public int? Nivel { get; set; }
        public int? IdEmocion { get; set; }
        public int? IdEntrada { get; set; }

        public virtual Emocion Emocion { get; set; }
        public virtual Entrada Entrada { get; set; }
    }
}
