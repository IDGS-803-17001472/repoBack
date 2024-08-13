namespace AuthAPI903.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string RolName { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }

    }
}
