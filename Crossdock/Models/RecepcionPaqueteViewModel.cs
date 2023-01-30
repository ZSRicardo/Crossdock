using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class DatosGuiaViewModel
    {
        //proceso recepcion de paquetes
        public int GuiaID { get; set; }

        public string Guia { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string Medida { get; set; }

        public double Peso { get; set; }

        public string Descripcion { get; set; }

        public string DestinatarioNombre { get; set; }

        public string DestinatarioApellidoP { get; set; }

        public string DestinatarioApellidoM { get; set; }

        public long DestinatarioCelular { get; set; }

        public string DestinatarioEmail { get; set; }

        public string DestinatarioCalle { get; set; }

        public string DestinatarioNumeroExt { get; set; }

        public string DestinatarioNumeroInt { get; set; }

        public string DestinatarioDireccion { get; set; }

        public string DestinatarioCodigoPostal { get; set; }

        public string NumeroIntento { get; set; }

        public string EstatusPaqueteID { get; set; }

        public string paqID { get; set; }

        public int Detalles { get; set; }

        public int Zonaid { get; set; }

        public string Zona { get; set; }

        public string Cliente { get; set; }
    }
}