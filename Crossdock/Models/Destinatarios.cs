namespace Crossdock.Models
{
    public class Destinatarios
    {
        //destinatarios_tb
        public int DestinatarioID { get; set; }

        public string Nombre { get; set; }

        public string ApellidoP { get; set; }

        public string ApellidoM { get; set; }

        public long Celular { get; set; }

        public string Email { get; set; }

        public string Calle { get; set; }

        public string NumeroExt { get; set; }

        public string NumeroInt { get; set; }

        public string Colonia { get; set; }

        public string CodigoPostal { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }
    }
}