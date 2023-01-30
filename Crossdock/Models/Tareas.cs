using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crossdock.Models
{
    public class Tareas
    {
        //tareas
        public int Tarea_ID { get; set; }
        public int Intento { get; set; }
        public DateTime Fecha_Asigacion { get; set; }
        public string fechaAsignacion { get; set; }
        public string Guia { get; set; }
        public string Estatus { get; set; }
        public int Usuario_ID { get; set; }
        public string Usuario_Identificador { get; set; }
        public string Celular { get; set; }
        public string Comentarios { get; set; }
        public int Tipo_Tarea { get; set; }
        public string Latitud { get; set; }
        public string longitud { get; set; }
        public string Usuario_Nombre { get; set; }
        public int Zona_ID { get; set; }
        public int Numero_Tareas { get; set; }
        public string CodigoPostal { get; set; }
        public string Des_Zona { get; set; }
        public string Tipo_Tarea_Descripcion { get; set; }
        public string C_Razon_Social { get; set; }
        public string Direccion_Destinatario { get; set; }
        public string Nombre_Destinatario { get; set; }
        public string Tarea_Activo { get; set; }
        public DateTime? Fecha_Aceptada { get; set; }
        public DateTime? Fecha_Fin { get; set; }
        public string StringFecha_Aceptada { get; set; }
        public string StringFecha_Fin { get; set; }
        public string Evidencia { get; set; }
        public int ID_Temporal { get; set; }
        public string Palabra_Clave { get; set; }
    }
}