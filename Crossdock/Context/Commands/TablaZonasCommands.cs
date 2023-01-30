using Crossdock.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaZonasCommands : CrossDockContext
    {
        /// <summary>
        /// Da de alta un nuevo registro Zonas en la base de datos. Genera un nuevo registro si se ingresa el id en "0", si no, modifica el registro existente.
        /// </summary>
        public void Alta_Zonas(Zonas Zona)
        {
            //Conexión a la base de datos //Writer porque Altas son escrituras
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Alta
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_zonas_sp";

                /*Pasar las propiedades a los parametros del SP*/
                /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                /*cmd.Parameters.AddWithValue("(nombre del parametro en el SP)", OBJECT.PARAMETER);*/

                cmd.Parameters.AddWithValue("zonid", Zona.ZonaID);
                cmd.Parameters.AddWithValue("zondescripcion", Zona.Descripcion);

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }


        /// <summary>
        /// Muestra todos los registros Zonas activos.
        /// </summary>
        public List<Zonas> Muestra_Zonas()
        {
            List<Zonas> List = new List<Zonas>();
            //Conexión a la base de datos //Reader porque es solo lectura
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base};convert zero datetime=True";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Muestra
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_zonas_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                //Ciclo que llena la lista de datos
                while (leer.Read())
                {
                    List.Add(new Zonas()
                    {
                        /*Igualar las propiedades con los parametros del SP*/
                        /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                        ZonaID = leer.GetInt32("zon_id"),
                        Descripcion=leer["zon_descripcion"].ToString(),
                    });
                }

                // Cierre General
                conexion.Close();//Cierra conexion
                leer.Close();//Cierra lista
                return List;//Devuelve la lista con datos
            }
        }

        /// <summary>
        /// Muestra un registro Zonas con base en un id.
        /// </summary>
        public List<Zonas> Muestra_ZonasMod(int id)
        {
            List<Zonas> List = new List<Zonas>();
            //Conexión a la base de datos //Reader porque es solo lectura
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales MuestraMod
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_zonasmod_sp";

                /*Igualar las propiedades con los parametros del SP*/
                /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                cmd.Parameters.AddWithValue("zonid", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                //Ciclo que llena la lista de datos
                while (leer.Read())
                {
                    List.Add(new Zonas()
                    {
                        /*Igualar las propiedades con los parametros del SP*/
                        /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                        ZonaID=leer.GetInt32("zon_id"),
                        Descripcion = leer["zon_descripcion"].ToString(),
                    });
                }

                // Cierre General
                conexion.Close();//Cierra conexion
                leer.Close();//Cierra lista
                return List;//Devuelve la lista con datos
            }
        }

        /// <summary>
        /// Borrado logico de un registro Zonas en la base de datos.
        /// </summary>
        public void Elimina_Zonas(int id)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Elimina
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_zonas_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("zonid", id);

                // Cierre General

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();//Cierra conexion
                cmd = null;
            }
        }
    }
}