using System.ComponentModel.DataAnnotations;

namespace AuthAPI903.Dtos
{
    public class AsignarProfesionalDto
    {
        [Required]
        public string CorreoProfesional { get; set; }
    }
}
