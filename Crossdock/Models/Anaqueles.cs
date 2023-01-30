using System.ComponentModel.DataAnnotations;

namespace Crossdock.Models
{
    public class Anaqueles
    {
        //Anaquetes_tb
        public int AnaquelID { get; set; }
        [Required]
        public string Descripcion { get; set; }
    }
}