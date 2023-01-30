using Crossdock.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaCondicionesCommands : CrossDockContext
    {
        public void Alta_Condiciones(Condiciones condiciones)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_condiciones_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("co_id", condiciones.CondicionID);
                cmd.Parameters.AddWithValue("co_descripcion", condiciones.Descripcion);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }//Genera un nuevo registro si se ingresa el id en 0 si no modifica los datos

        public List<Condiciones> Muestra_Condiciones()//muestra todos los comdiciones activos
        {
            List<Condiciones> List = new List<Condiciones>();
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType=CommandType.StoredProcedure;
                cmd.CommandText = "muestra_condiciones_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Condiciones()//llena la lista de datos
                    {
                        CondicionID = leer.GetInt32("con_id"),
                        Descripcion = leer["con_descripcion"].ToString(),
                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public List<Condiciones> Muestra_CondicionesMod(int id)//muestra una condiciones basados en el id
        {
            List<Condiciones> List = new List<Condiciones>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_condicionesmod_sp";
                // Parametros de SP
                cmd.Parameters.AddWithValue("co_id", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Condiciones()//llena la lista de datos
                    {
                        CondicionID=leer.GetInt32("con_id"),
                        Descripcion = leer["con_descripcion"].ToString(),
                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public void Elimina_Condiciones(int id)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_condiciones_tb";
                cmd.Parameters.AddWithValue("co_id", id);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }
    }
}