using Crossdock.Context;
using Crossdock.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;

namespace Crossdock.Context.Commands 
{ 
    public class TablaRolesCommands : CrossDockContext
    {
        /// <summary>
        /// Da de alta un nuevo registro Roles en la base de datos. Genera un nuevo registro si se ingresa el id en "0", si no, modifica el registro existente.
        /// </summary>
        public void Alta_Roles(Roles Rol)
        {
            //Conexión a la base de datos //Writer porque Altas son escrituras
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Alta
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_roles_sp";

                /*Pasar las propiedades a los parametros del SP*/
                /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                /*cmd.Parameters.AddWithValue("(nombre del parametro en el SP)", OBJECT.PARAMETER);*/
                cmd.Parameters.AddWithValue("rolid", Rol.RolID);
                cmd.Parameters.AddWithValue("roldescripcion", Rol.Descripcion);
                cmd.Parameters.AddWithValue("rolpermisos", Rol.Permisos);

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }

        /// <summary>
        /// Muestra todos los registros Roles activos.
        /// </summary>
        public List<Roles> Muestra_Roles()
        {
            List<Roles> List = new List<Roles>();
            //Conexión a la base de datos //Reader porque es solo lectura
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Muestra
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_roles_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                //Ciclo que llena la lista de datos
                while (leer.Read())
                {
                    List.Add(new Roles()
                    {
                        /*Igualar las propiedades con los parametros del SP*/
                        /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                        RolID = leer.GetInt32("rol_id"),
                        Descripcion=leer["rol_descripcion"].ToString(),
                        Permisos = leer["rol_permisos"].ToString(),

                });
                }

                // Cierre General
                conexion.Close();//Cierra conexion
                leer.Close();//Cierra lista
                return List;//Devuelve la lista con datos
            }
        }

        /// <summary>
        /// Muestra un registro Roles con base en un id.
        /// </summary>
        public List<Roles> Muestra_RolesMod(int id)
        {
            List<Roles> List = new List<Roles>();

            //Conexión a la base de datos //Reader porque es solo lectura
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales MuestraMod
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_rolesmod_sp";

                /*Igualar las propiedades con los parametros del SP*/
                /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                cmd.Parameters.AddWithValue("rolid", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Roles()
                    {
                        RolID = leer.GetInt32("rol_id"),
                        Descripcion = leer["rol_descripcion"].ToString(),
                        Permisos = leer["rol_permisos"].ToString(),
                    });
                }

                // Cierre General
                conexion.Close();//Cierra conexion
                leer.Close();
                return List;//Devuelve la lista con datos
            }
        }

        /// <summary>
        /// Borrado logico de un registro Roles en la base de datos.
        /// </summary>
        public void Elimina_Roles(int id)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Elimina
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_roles_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("rolid", id);

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();//Cierra conexion
                cmd = null;
            }
        }
    }
}