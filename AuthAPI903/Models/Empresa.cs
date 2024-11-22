namespace AuthAPI903.Models
{
    public class Empresa
    {
        public int Id { get; set; }  // Auto-increment
        public string NombreEmpresa { get; set; }
        public string Direccion { get; set; }
        public string NombreCliente { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public int Estatus { get; set; }
    }
}
