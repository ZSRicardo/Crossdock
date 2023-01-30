using Crossdock.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaAnaquelesCommands : CrossDockContext
    {
        public void Alta_Anaqueles(Anaqueles anaqueles)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_anaqueles_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("an_id", anaqueles.AnaquelID);
                cmd.Parameters.AddWithValue("an_desc", anaqueles.Descripcion);

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }//Genera un nuevo registro si se ingresa el id en 0 si no modifica los datos

        public List<Anaqueles> Muestra_Anaqueles()//muestra todos los anaqueles activos
        {
            List<Anaqueles> List = new List<Anaqueles>();
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_anaqueles_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Anaqueles()//llena la lista de datos
                    {
                        AnaquelID = leer.GetInt32("ana_id"),
                        Descripcion = leer["ana_descripcion"].ToString(),
                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public List<Anaqueles> Muestra_AnaquelesMod(Anaqueles anaqueles)//muestra un anaquel basado en el id
        {
            List<Anaqueles> List = new List<Anaqueles>();
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_anaquelesmod_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("an_id", anaqueles.AnaquelID);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Anaqueles()//llena la lista de datos
                    {
                        AnaquelID = leer.GetInt32("ana_id"),
                        Descripcion = leer["ana_descripcion"].ToString(),
                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public void Elimina_Anaqueles(Anaqueles anaqueles)
        {

            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_anaqueles_sp";

                // Parametros de SP

                cmd.Parameters.AddWithValue("an_id", anaqueles.AnaquelID);
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }

        }//borrado logico de un registro
    }
}