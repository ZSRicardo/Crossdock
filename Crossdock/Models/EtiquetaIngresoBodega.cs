using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crossdock.Models
{
    public class EtiquetaIngresoBodega
    {
        //model para etiquetas
     public string Numero_Pedido { get; set; }
     public string Zona { get; set; }
     public string Remitente { get; set; }
     public string Destinatario { get; set; }
     public  string Guia { get; set; }
     public DateTime Fecha_Recepcion { get; set; }
     public string Direccion { get; set; }
     public int Paqueteria { get;set; }
    }
}