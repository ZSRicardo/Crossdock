using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaRegresosCommands : CrossDockContext
    {
        /// <summary>
        /// Da de alta un nuevo registro Regresos en la base de datos. Genera un nuevo registro si se ingresa el id en "0", si no, modifica el registro existente.
        /// </summary>
        public void Alta_Regresos(Regresos Regreso)
        {
            //Conexión a la base de datos //Writer porque Altas son escrituras
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Alta
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_regresos_sp";

                /*Pasar las propiedades a los parametros del SP*/
                /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                /*cmd.Parameters.AddWithValue("(nombre del parametro en el SP)", OBJECT.PARAMETER);*/

                cmd.Parameters.AddWithValue("bo_id", Regreso.RegresoID);
                cmd.Parameters.AddWithValue("bo_nombre", Regreso.ClienteID);
                cmd.Parameters.AddWithValue("bo_email", Regreso.FechaReg);
                cmd.Parameters.AddWithValue("bo_telefono", Regreso.PaqueteID);
                cmd.Parameters.AddWithValue("bo_calle", Regreso.UsuarioID);

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }


        /// <summary>
        /// Muestra todos los registros Regresos activos.
        /// </summary>
        public List<Regresos> Muestra_Regresos()
        {
            List<Regresos> List = new List<Regresos>();
            //Conexión a la base de datos //Reader porque es solo lectura
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Muestra
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_regresos_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                //Ciclo que llena la lista de datos
                while (leer.Read())
                {
                    List.Add(new Regresos()
                    {
                        /*Igualar las propiedades con los parametros del SP*/
                        /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                        RegresoID = leer.GetInt32("reg_id"),
                        ClienteID=Convert.ToInt32(leer["cli_id"].ToString()),
                        FechaReg= Convert.ToDateTime(leer["reg_fecha"].ToString()),
                        PaqueteID=Convert.ToInt32(leer["paq_id"].ToString()),
                        UsuarioID=Convert.ToInt32(leer["usu_id"].ToString()),
                    });
                }

                // Cierre General
                conexion.Close();//Cierra conexion
                leer.Close();//Cierra lista
                return List;//Devuelve la lista con datos
            }
        }

        /// <summary>
        /// Borrado logico de un registro Regresos en la base de datos.
        /// </summary>
        public void Elimina_Regresos(Regresos Regreso)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Elimina
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_regresos_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("regid", Regreso.RegresoID);

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();//Cierra conexion
                cmd = null;
            }
        }
    }
}