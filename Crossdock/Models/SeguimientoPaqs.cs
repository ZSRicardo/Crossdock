using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class SeguimientoPaqs
    {
        public string Guia { get; set; }
        public string FechaRecepcion { get; set; }
        public string Estatus { get; set; }
        public string FechaEntrega { get; set; }
        public string EvidenciaEntrega { get; set; }

    }
}