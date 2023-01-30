using Crossdock.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaUnidadesCommands : CrossDockContext
    {
        /// <summary>
        /// Da de alta un nuevo registro Unidades en la base de datos. Genera un nuevo registro si se ingresa el id en "0", si no, modifica el registro existente.
        /// </summary>
        public void Alta_Unidades(Unidades Unidad)
        {
            //Conexión a la base de datos //Writer porque Altas son escrituras
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Alta
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_unidades_sp";

                /*Pasar las propiedades a los parametros del SP*/
                /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                /*cmd.Parameters.AddWithValue("(nombre del parametro en el SP)", OBJECT.PARAMETER);*/

                cmd.Parameters.AddWithValue("uniid", Unidad.UnidadID);
                cmd.Parameters.AddWithValue("unimodelo", Unidad.Modelo);
                cmd.Parameters.AddWithValue("unimarca", Unidad.Marca);
                cmd.Parameters.AddWithValue("uniplacas", Unidad.Placas);
                cmd.Parameters.AddWithValue("tunid", Unidad.TipoUnidadID);

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }

        /// <summary>
        /// Muestra todos los registros Unidades activos.
        /// </summary>
        public List<Unidades> Muestra_Unidades()
        {
            List<Unidades> List = new List<Unidades>();
            //Conexión a la base de datos //Reader porque es solo lectura
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Muestra
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "muestra_unidades_sp"
                };
                conexion.Open();
                var leer = cmd.ExecuteReader();
                while (leer.Read())
                {
                    List.Add(new Unidades()
                    {
                        UnidadID = leer.GetInt32("uni_id"),
                        Modelo=leer["uni_modelo"].ToString(),
                        Marca=leer["uni_marca"].ToString(),
                        Placas=leer["uni_placas"].ToString(),
                        TipoUnidadID = leer.GetInt32("tun_id"),
                        TipoUnidadDescripcion = leer["tun_descripcion"].ToString(),
                    });
                }

                // Cierre General
                conexion.Close();//Cierra conexion
                leer.Close();//Cierra lista
                return List;//Devuelve la lista con datos
            }
        }

        //Agregar metodo para consumir procedimiento MuestraTipoUnidades

        /// <summary>
        /// Muestra un registro Unidades con base en un id.
        /// </summary>
        public List<Unidades> Muestra_UnidadesMod(int id)
        {
            List<Unidades> List = new List<Unidades>();
            //Conexión a la base de datos //Reader porque es solo lectura
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales MuestraMod
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "muestra_unidadesmod_sp"
                };

                /*Igualar las propiedades con los parametros del SP*/
                /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                cmd.Parameters.AddWithValue("uniid", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                //Ciclo que llena la lista de datos
                while (leer.Read())
                {
                    List.Add(new Unidades()
                    {
                        /*Igualar las propiedades con los parametros del SP*/
                        /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                        UnidadID = leer.GetInt32("uni_id"),
                        Modelo = leer["uni_modelo"].ToString(),
                        Marca = leer["uni_marca"].ToString(),
                        Placas = leer["uni_placas"].ToString(),
                        TipoUnidadID = leer.GetInt32("tun_id"),
                        TipoUnidadDescripcion = leer["tun_descripcion"].ToString(),
                    });
                }

                // Cierre General
                conexion.Close();//Cierra conexion
                leer.Close();//Cierra lista
                return List;//Devuelve la lista con datos
            }
        }

        /// <summary>
        /// Borrado logico de un registro Unidades en la base de datos.
        /// </summary>
        public void Elimina_Unidades(int id)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Elimina
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "elimina_unidades_sp"
                };

                // Parametros de SP
                cmd.Parameters.AddWithValue("uniid", id);

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();//Cierra conexion
                cmd = null;
            }
        }
    }
}