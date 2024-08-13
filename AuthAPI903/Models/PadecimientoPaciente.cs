namespace AuthAPI903.Models
{
    public class PadecimientoPaciente
    {
        public int Id { get; set; }
        public int? IdPadecimiento { get; set; }
        public int? IdPaciente { get; set; }

        public virtual Padecimiento Padecimiento { get; set; }
        public virtual Paciente Paciente { get; set; }
    }
}
