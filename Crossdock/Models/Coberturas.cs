using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class Coberturas
    {
        [Key]
        [Column(Order = 0)]
        public int CoberturaID { get; set; }

        [Display(Name = "Colonia")]
        public string Colonia { get; set; }

        [Display(Name = "Código Postal")]
        public string CodigoPostal { get; set; }

        [Display(Name = "Descripcion")]
        public string ZonaDescripcion { get; set; }

        [Display(Name = "Nombre")]
        public string DeliveryNombre { get; set; }

        [Display(Name = "Telefono")]
        public string DeliveryTelefono { get; set; }

        [Display(Name = "Delivery")]
        public int DeliveryID { get; set; }

        [Display(Name = "Zona")]
        public int ZonaID { get; set; }
        public int ID_Temporal { get; set; }
    }
}