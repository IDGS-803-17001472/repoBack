namespace AuthAPI903.Dtos
{
    public class UserDetailDto
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public List<string> Roles { get; set; }
        public string? PhoneNumber { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int AccessFailedCount { get; set; }
        public string? Foto { get; set; }
        public PersonaDto? Persona { get; set; }



        public ProfesionalDto? Profesional { get; set; }
    }
}