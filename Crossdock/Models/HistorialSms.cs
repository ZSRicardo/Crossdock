using System;

namespace Crossdock.Models
{
    public class HistorialSms
    {

        //historial_sms
        public int HistorialSmsID { get; set; }
        public DateTime Fecha { get; set; }
        public string Mensaje { get; set; }
        public string Guia { get; set; }
        public string DestinatarioNombre { get; set; }
        public string DestinatarioApellidoP { get; set; }
        public string DestinatarioApellidoM { get; set; }
        public string UsuarioNombre { get; set; }


    }
}