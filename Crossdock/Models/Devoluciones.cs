namespace Crossdock.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Devoluciones
    {
        [Key]
        [Column(Order = 0)]
        public int DevolucionID { get; set; }

        [Display(Name = "Detalles")]
        public string Detalles { get; set; }
        public int ID_Temporal { get; set; }
    }
}