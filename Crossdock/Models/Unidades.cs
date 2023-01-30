using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class Unidades
    {
        //Unidades_tb
        [Key]
        [Column(Order = 0)]
        public int UnidadID { get; set; }

        [Display(Name = "Modelo de la Unidad")]
        public string Modelo { get; set; }

        [Display(Name = "Marca de la Unidad")]
        public string Marca { get; set; }

        [Display(Name = "Placas de la Unidad")]
        public string Placas { get; set; }

        [Display(Name = "Tipo de unidad")]
        public int TipoUnidadID { get; set; }

        [Display(Name = "Descripción Unidad")]
        public string TipoUnidadDescripcion { get; set; }
        public int ID_Temporal { get; set; }
    }
}