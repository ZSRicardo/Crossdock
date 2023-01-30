using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crossdock.Models
{
    public class JsonToModel
    {
        public guia[] Guias { get; set; }
    }

    public class guia
    {
        public string Numero_Pedido { get; set; }
        public string Nombre_Destinatario { get; set; }
        public string Apellido_Paterno_Destinatario { get; set; }
        public string Apellido_Materno_Destinatario { get; set; }
        public string Celular_Destinatario { get; set; }
        public string Email { get; set; }
        public string Calle { get; set; }
        public string Numero_Exterior { get; set; }
        public string Numero_Interior { get; set; }
        public string Colonia { get; set; }
        public string Codigo_Postal { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Medida_Paquete { get; set; }
        public string Peso_paquete { get; set; }
        public string Descripcion_Paquete { get; set; }
        public string Instrucciones_Paquete { get; set; }
        public string Tipo_Servicio { get; set; }
        public string Total_de_Paquetes { get; set; }

    }

}