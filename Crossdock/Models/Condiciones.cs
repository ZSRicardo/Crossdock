using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class Condiciones
    {
        //condiciones_tb
        [Key]
        [Column(Order = 0)]
        public int CondicionID { get; set; }

        [Display(Name = "Condición")]
        public string Descripcion { get; set; }

        public int ID_Temporal { get; set; }

    }
}