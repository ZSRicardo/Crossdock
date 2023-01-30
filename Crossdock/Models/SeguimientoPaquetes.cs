using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class SeguimientoPaquetes
    {
        [Key]
        [Column(Order = 0)]

        [Display(Name = "Fecha de recepción")]
        public DateTime FechaRecepcion { get; set; }
        public string fechaRecepcion { get; set; }

        [Display(Name = "Estatus")]
        public string Estatus { get; set; }

        [Display(Name = "Evidencia de entrega")]
        public string EvidenciaEntrega { get; set; }

        [Display(Name = "Fecha de entrega")]
        public DateTime? FechaEntrega { get; set; }
        public string fechaEntrega { get; set; }

        [Display(Name = "Guía")]
        public string Guia { get; set; }

        [Display(Name = "Destinatario")]
        public string NombreDestinatario { get; set; }

        [Display(Name = "Dirección de entrega")]
        public string Direccion { get; set; }

        [Display(Name = "Condición")]
        public string Condicion { get; set; }

        [Display(Name = "Palabra Clave")]
        public string PalabraClave { get; set; }

        [Display(Name = "Numero de intentos")]
        public string Nintentos { get; set; }

        public int ClienteID { get; set; }

        public string Cliente { get; set; }

        public int PaqueteID { get; set; }

        public string RazonSocial { get; set; }

        public int ID_Temporal { get; set; }

    }
}