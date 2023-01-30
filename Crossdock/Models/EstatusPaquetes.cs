using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class EstatusPaquetes
    {
        //estatus_paquetes
        [Key]
        [Column(Order = 0)]
        public int EstatusPaqueteID { get; set; }

        [Display(Name = "Estatus paquete")]
        public string Descripcion { get; set; }
        public int ID_Temporal { get; set; }
    }
}