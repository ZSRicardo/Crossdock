using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class TipoEntregas
    {
        //tipo_entregas
        [Key]
        [Column(Order = 0)]
        [Display(Name = "Nombre del Operador")]
        public int TipoEntregasID { get; set; }

        [Display(Name = "Tipo de entrega")]
        public string Descripcion { get; set; }
        public int ID_Temporal { get; set; }
    }
}