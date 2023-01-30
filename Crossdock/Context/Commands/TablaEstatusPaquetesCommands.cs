using Crossdock.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaEstatusPaquetesCommands : CrossDockContext
    {
        public void Alta_EstatusPaquetes(EstatusPaquetes estatus)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_estatus_paquetes_sp";
                cmd.Parameters.AddWithValue("ep_id", estatus.EstatusPaqueteID);
                cmd.Parameters.AddWithValue("ep_descripcion", estatus.Descripcion);


                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }

        public List<EstatusPaquetes> Muestra_EstatusPaquetes()//muestra todos los registros activos
        {
            List<EstatusPaquetes> List = new List<EstatusPaquetes>();

            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_estatus_paquetes_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new EstatusPaquetes()
                    {
                        EstatusPaqueteID = leer.GetInt32("epa_id"),
                        Descripcion = leer["epa_descripcion"].ToString(),

                    });
                }

                conexion.Close();
                leer.Close();
                return List;
            }
        }

        public List<EstatusPaquetes> Muestra_EstatusPaquetesMod(int id)
        {
            List<EstatusPaquetes> List = new List<EstatusPaquetes>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_estatus_paquetesmod_sp";

                cmd.Parameters.AddWithValue("ep_id", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new EstatusPaquetes()
                    {
                        EstatusPaqueteID = leer.GetInt32("epa_id"),
                        Descripcion = leer["epa_descripcion"].ToString(),
                    });
                }
                conexion.Close();
                leer.Close();
                return List;
            }
        }

        public void Elimina_EstatusPaquetes(int id)
        {

            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_estatus_paquetes_sp";
                cmd.Parameters.AddWithValue("ep_id", id);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }

        }
    }
}