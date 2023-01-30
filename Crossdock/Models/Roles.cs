using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class Roles
    {
        //roles
        [Key]
        [Column(Order = 0)]
        public int RolID { get; set; }
        [Display(Name = "Tipo de Usuario")]
        public string Descripcion { get; set; }
        [Display(Name = "Permisos")]
        public string Permisos { get; set; }

        //Permisos Guías
        public bool GeneracionGuias { get; set; }
                
        //Permisos CEDIS
        public bool RecepcionPaquetes { get; set; }
        //Permisos CEDIS
        public bool Notificaciones { get; set; }

        //Permisos Área
        public bool AsignacionTareas { get; set; }

        //Permisos Paquetes
        public bool SeguimientoPaquetes { get; set; }

        //Permisos Historial Paquetes
        public bool HistorialPaquetes { get; set; }

        //Permisos Guias Prepagadas
        public bool GuiasPrepagadas { get; set; }

        //Permisos Usuarios
        public bool UsuariosVisualizar { get; set; }
        public bool UsuariosCrear { get; set; }
        public bool UsuariosEliminar { get; set; }
        public bool UsuariosEditar { get; set; }

        //Permisos Bodegas
        public bool BodegasVisualizar { get; set; }
        public bool BodegasCrear { get; set; }
        public bool BodegasEliminar { get; set; }
        public bool BodegasEditar { get; set; }

        //Permisos Clientes
        public bool ClientesVisualizar { get; set; }
        public bool ClientesCrear { get; set; }
        public bool ClientesEliminar { get; set; }
        public bool ClientesEditar { get; set; }

        //Permisos Condiciones
        public bool CondicionesVisualizar { get; set; }
        public bool CondicionesCrear { get; set; }
        public bool CondicionesEliminar { get; set; }
        public bool CondicionesEditar { get; set; }

        //Permisos Delivery
        public bool DeliveryVisualizar { get; set; }
        public bool DeliveryCrear { get; set; }
        public bool DeliveryEliminar { get; set; }
        public bool DeliveryEditar { get; set; }

        //Permisos Estatus Paquetes
        public bool EstatusPaVisualizar { get; set; }
        public bool EstatusPaCrear { get; set; }
        public bool EstatusPaEliminar { get; set; }
        public bool EstatusPaEditar { get; set; }

        //Permisos Estatus Tareas
        public bool EstatusTaVisualizar { get; set; }
        public bool EstatusTaCrear { get; set; }
        public bool EstatusTaEliminar { get; set; }
        public bool EstatusTaEditar { get; set; }

        //Permisos Operadores
        public bool OperadoresVisualizar { get; set; }
        public bool OperadoresCrear { get; set; }
        public bool OperadoresEliminar { get; set; }
        public bool OperadoresEditar { get; set; }

        //Permisos Tipo de Operador
        public bool TopVisualizar { get; set; }
        public bool TopCrear { get; set; }
        public bool TopEliminar { get; set; }
        public bool TopEditar { get; set; }

        //Permisos Roles
        public bool RolesVisualizar { get; set; }
        public bool RolesCrear { get; set; }
        public bool RolesEliminar { get; set; }
        public bool RolesEditar { get; set; }

        //Permisos Tipo de Entrega
        public bool TenVisualizar { get; set; }
        public bool TenCrear { get; set; }
        public bool TenEliminar { get; set; }
        public bool TenEditar { get; set; }

        //Permisos Unidades
        public bool UnidadesVisualizar { get; set; }
        public bool UnidadesCrear { get; set; }
        public bool UnidadesEliminar { get; set; }
        public bool UnidadesEditar { get; set; }

        //Permisos Tipo de Unidad
        public bool TunVisualizar { get; set; }
        public bool TunCrear { get; set; }
        public bool TunEliminar { get; set; }
        public bool TunEditar { get; set; }

        //Permisos Zonas
        public bool ZonasVisualizar { get; set; }
        public bool ZonasCrear { get; set; }
        public bool ZonasEliminar { get; set; }
        public bool ZonasEditar { get; set; }

        //Permisos Motivos de Devolución
        public bool MotivosVisualizar { get; set; }
        public bool MotivosCrear { get; set; }
        public bool MotivosEliminar { get; set; }
        public bool MotivosEditar { get; set; }

        //Permisos Coberturas
        public bool CoberturasVisualizar { get; set; }
        public bool CoberturasCrear { get; set; }
        public bool CoberturasEliminar { get; set; }
        public bool CoberturasEditar { get; set; }

        public int ID_Temporal { get; set; }

        //Permisos Historial de tareas
        public bool HistorialTareas { get; set; }

        //Permisos Historial de guias
        public bool HistorialGuias { get; set; }


    }
}