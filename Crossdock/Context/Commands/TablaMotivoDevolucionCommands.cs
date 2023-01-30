using Crossdock.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaMotivoDevolucionCommands : CrossDockContext
    {
        public void Alta_Devoluciones(Devoluciones devolucion)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_devoluciones_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("de_id", devolucion.DevolucionID);
                cmd.Parameters.AddWithValue("de_detalles", devolucion.Detalles);

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }//Genera un nuevo registro si se ingresa el id en 0 si no modifica los datos

        public List<Devoluciones> Muestra_Devoluciones()//muestra todos los devolucion activos
        {
            List<Devoluciones> List = new List<Devoluciones>();
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_devoluciones_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Devoluciones()//llena la lista de datos
                    {
                        DevolucionID = leer.GetInt32("dev_id"),
                        Detalles = leer["dev_detalles"].ToString(),
                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public List<Devoluciones> Muestra_DevolucionesMod(int id)//muestra un anaquel basado en el id
        {
            List<Devoluciones> List = new List<Devoluciones>();
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_devolucionesmod_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("de_id", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Devoluciones()//llena la lista de datos
                    {
                        DevolucionID = leer.GetInt32("dev_id"),
                        Detalles = leer["dev_detalles"].ToString(),
                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public void Elimina_Devoluciones(int id)
        {

            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_devoluciones_sp";

                // Parametros de SP

                cmd.Parameters.AddWithValue("de_id", id);
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }

        }//borrado logico de un registro
    }
}