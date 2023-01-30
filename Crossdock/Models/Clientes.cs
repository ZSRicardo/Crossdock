using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class Clientes
    {
        //clientes_tb
        [Key]
        [Column(Order = 0)]
        public int ClienteID { get; set; }

        [Display(Name = "Razón social")]
        public string RazonSocial { get; set; }

        // Nombre de contacto
        [Display(Name = "Contacto")]
        public string Contacto { get; set; }

        [Display(Name = "Celular")]
        public long Telefono { get; set; }

        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Display(Name = "Calle")]
        public string Calle { get; set; }

        [Display(Name = "Numero exterior")]
        public string NumeroExt { get; set; }

        [Display(Name = "Numero interior")]
        public string NumeroInt { get; set; }

        [Display(Name = "Colonia")]
        public string Colonia { get; set; }

        [Display(Name = "Código postal")]
        public string CodigoPostal { get; set; }

        [Display(Name = "Precio Servicio")]
        public int PreciosServiciosID { get; set; }

        [Display(Name = "Identificador cliente")]
        public string ClienteIdentificador { get; set; }

        public int ID_Temporal { get; set; }
    }
}