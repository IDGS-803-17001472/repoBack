namespace AuthAPI903.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string CorreoElectronico { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaDeRegistro { get; set; }
    }

    public class RegistrarClienteDto
    {
        public string CorreoElectronico { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }

    public class ActualizarClienteDto
    {
        public string CorreoElectronico { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
