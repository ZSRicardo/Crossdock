using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Crossdock.Models
{
    public class GuiaPrepagada
    {
        public int GPr_ID { get; set; }
        public int ClienteID { get; set; }

        [Required(ErrorMessage = "Por favor seleccione un cliente")]
        [Display(Name = "Cliente:")]
        public string Cliente_RazonSocial { get; set; }
        public string Cliente_Identificador { get; set; }
        public int PrecioServicioID { get; set; }
        [Required(ErrorMessage = "Por favor ingrese la cantidad de guias a asignar")]
        [Display(Name = "Numero de Guias:")]
        public int NumeroGuias { get; set; }
        public DateTime? FechaCompra { get; set; }
        [Required(ErrorMessage = "Por favor seleccione la fecha de vencimiento")]
        [Display(Name = "Fecha de Vencimiento:")]
        public DateTime? FechaVenciminto { get; set; }
        public int ID_Temporal { get; set; }
        public int GuiasUsadas { get; set; }
    }
}