namespace AuthAPI903.Models
{
    public class Domicilio
    {
        public int Id { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public string NumeroInterior { get; set; }
        public string NumeroExterior { get; set; }
        public string CodigoPostal { get; set; }
        public int? IdPersona { get; set; }

        public virtual Persona Persona { get; set; }
    }
}
