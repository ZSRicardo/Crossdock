using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class TipoUnidades
    {
        //tipo_unidades_tb
        [Key]
        [Column(Order = 0)]
        public int TipoUnidadID { get; set; }

        [Display(Name = "Tipo de Unidad")]
        public string Descripcion { get; set; }
        public int ID_Temporal { get; set; }
    }
}