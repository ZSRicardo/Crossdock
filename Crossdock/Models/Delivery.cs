using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class Delivery
    {
        //delivery_tb
        [Key]
        [Column(Order = 0)]
        public int DeliveryID { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Celular")]
        public Int64 Telefono { get; set; }

        [Display(Name = "Calle")]
        public string Calle { get; set; }

        [Display(Name = "Número exterior")]
        public string NumeroExt { get; set; }

        [Display(Name = "Número interior")]
        public string NumeroInt { get; set; }

        [Display(Name = "Colonia")]
        public string Colonia { get; set; }

        [Display(Name = "Código postal")]
        public string CodigoPostal { get; set; }

        [Display(Name = "Precio de servicio")]
        public string PreciosServiciosID { get; set; }

        [Display(Name = "Delivery Identificador")]
        public string DeliveryIdentificador { get; set; }
        public int ID_Temporal { get; set; }
    }
}