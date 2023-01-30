using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class Zonas
    {
        //Zonas_tb
        [Key]
        [Column(Order = 0)]
        public int ZonaID { get; set; }
        public string Descripcion { get; set; }
        public int ID_Temporal { get; set; }
    }
}