using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Crossdock.Context.Commands
{
    public class ConsultaColonias: CrossDockContext
    {
        public List<string> consultaColonias(string codigoPostal)//muestra un registro basado en el id
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            List<string> Colonias = new List<string>();
            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
              
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_zona_sp";//modificar nombre del delegaciobn
                // Parametros de SP
                cmd.Parameters.AddWithValue("codigopostal", codigoPostal);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                try
                {
                    while (leer.Read())
                    {
                        Colonias.Add(leer["zon_descripcion"].ToString());
                    }
                    conexion.Close();//cierra conexion
                    leer.Close();
                    return Colonias;
                }
                catch (Exception e)
                {
                    conexion.Close();//cierra conexion
                    leer.Close();
                    return Colonias;
                }


            }
        }
    }
}