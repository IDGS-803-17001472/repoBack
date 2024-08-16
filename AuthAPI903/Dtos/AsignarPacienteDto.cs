using System.ComponentModel.DataAnnotations;


namespace AuthAPI903.Dtos
{
    public class AsignarPacienteDto
    {
        [Required]
        public int? pacienteId { get; set; } = null;
    }
}