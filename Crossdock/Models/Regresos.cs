using System;

namespace Crossdock.Models
{
    public class Regresos
    {
        //regresos
        public int RegresoID { get; set; }
        public int ClienteID { get; set; }
        public DateTime? FechaReg { get; set; }
        public int PaqueteID { get; set; }
        public int UsuarioID { get; set; }
    }
}