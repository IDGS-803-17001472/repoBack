namespace AuthAPI903.Models
{
    public class Padecimiento
    {
        public int Id { get; set; }
        public string NombrePadecimiento { get; set; }

        public virtual ICollection<PadecimientoPaciente> PadecimientoPacientes { get; set; }

    }
}
