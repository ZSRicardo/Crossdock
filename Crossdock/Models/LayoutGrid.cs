using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crossdock.Models
{
    public class LayoutGrid
    {
        //Layout
        public string NoPedido { get; set; }
        public string Destinatario { get; set; }
        public string DireccionDes { get; set; }
        public string InformacionPaq { get; set; }
        public string CodigoCliente { get; set; }
        public string CodigoUsuario { get; set; }
        public string CodigoDelivery { get; set; }
        public string Guia { get; set; }
        public int TServicio { get; set; }
        public int Paquetes_Extra { get; set; }
    }
}