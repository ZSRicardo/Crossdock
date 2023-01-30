using System;

namespace Crossdock.Models
{
    public class Paquetes
    {
        //proceso paquetes
        public int ID_Temporal { get; set; }
        public string p { get; set; }//seleccion solo en vista checkbox
        public int PaqueteID { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public string Guia { get; set; }
        public string CodigoRecoleccion { get; set; }
        public string FechaIngresoBodega { get; set; }
        public string FechaSalidaBodega { get; set; }
        public string FechaCargaOperador { get; set; }
        public string FechaEntregaFinal { get; set; }
        public string GPS { get; set; }
        public int Intento { get; set; }
        public string Foto { get; set; }
        public int OperadorID { get; set; }
        public int UsuarioID { get; set; }
        public int ClienteID { get; set; }
        public int EstatusPaqueteID { get; set; }
        public int BodegaID { get; set; }
        public int CondicionID { get; set; }
        public int ZonaID { get; set; }
        public int AnaquelID { get; set; }
        public int MotivoDevolucionID { get; set; }
        public int GuiID { get; set; }
        public string Zona { get; set; }
        public string Nom_Operador { get; set; }
        public string CodigoPostal { get; set; }
        public string EstatusPaqueteDescripcion { get; set; }
        public string Razon_Social_Cliente { get; set; }
        public string Paq_Descripcion { get; set; }
        public string Paq_Instrucciones { get; set; }
        public string Destinatario { get; set; }
        public string Direccion { get; set; }
        public string Fecha_Recepcion_String { get; set; }

        

    }
}