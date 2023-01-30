using Crossdock.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaTipoUnidadesCommands : CrossDockContext
    {
        /// <summary>
        /// Da de alta un nuevo registro TipoUnidades en la base de datos. Genera un nuevo registro si se ingresa el id en "0", si no, modifica el registro existente.
        /// </summary>
        public void Alta_TipoUnidades(TipoUnidades TipoUnidad)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Alta
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "alta_tipo_unidades_sp"
                };

                cmd.Parameters.AddWithValue("tunid", TipoUnidad.TipoUnidadID);
                cmd.Parameters.AddWithValue("tundescripcion", TipoUnidad.Descripcion);

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }


        /// <summary>
        /// Muestra todos los registros TipoUnidades activos.
        /// </summary>
        public List<TipoUnidades> Muestra_TipoUnidades()
        {
            List<TipoUnidades> List = new List<TipoUnidades>();
            //Conexión a la base de datos //Reader porque es solo lectura
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Muestra
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "muestra_tipo_unidades_sp"
                };
                conexion.Open();
                var leer = cmd.ExecuteReader();

                //Ciclo que llena la lista de datos
                while (leer.Read())
                {
                    List.Add(new TipoUnidades()
                    {
                        TipoUnidadID = leer.GetInt32("tun_id"),
                        Descripcion=leer["tun_descripcion"].ToString(),
                    });
                }
                conexion.Close();
                leer.Close();
                return List;
            }
        }

        /// <summary>
        /// Muestra un registro TipoUnidades con base en un id.
        /// </summary>
        public List<TipoUnidades> Muestra_TipoUnidadesMod(int id)
        {
            List<TipoUnidades> List = new List<TipoUnidades>();
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales MuestraMod
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "muestra_tipo_unidadesmod_sp"
                };

                cmd.Parameters.AddWithValue("tunid", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new TipoUnidades()
                    {
                        TipoUnidadID = leer.GetInt32("tun_id"),
                        Descripcion = leer["tun_descripcion"].ToString(),
                    });
                }

                conexion.Close();
                leer.Close();
                return List;
            }
        }

        /// <summary>
        /// Borrado logico de un registro TipoUnidades en la base de datos.
        /// </summary>
        public void Elimina_TipoUnidades(int id)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Elimina
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "elimina_tipo_unidades_sp"
                };

                // Parametros de SP
                cmd.Parameters.AddWithValue("tunid", id);

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();//Cierra conexion
                cmd = null;
            }
        }
    }
}