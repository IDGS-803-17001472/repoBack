namespace AuthAPI903.Models
{
    public class TableroRendimiento
    {
        public int Id { get; set; }
        public int LeadsConcretados { get; set; }
        public int LeadsCerrados { get; set; }
        public double TasaConversion { get; set; }
    }
}