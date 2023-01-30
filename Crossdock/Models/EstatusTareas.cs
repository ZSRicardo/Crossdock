using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class EstatusTareas
    {
        //estatus_tareas
        [Key]
        [Column(Order = 0)]
        public int EstatusTareasID { get; set; }

        [Display(Name = "Estatus tarea")]
        public string Descripcion { get; set; }
        public int ID_Temporal { get; set; }



    }
}