using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class TipoOperadores
    {
        //tipo_operadores
        [Key]
        [Column(Order = 0)]
        public int TipoOperadorID { get; set; }

        [Display(Name = "Tipo de Operador")]
        public string Descripcion { get; set; }

        public int ID_Temporal { get; set; }
    }
}