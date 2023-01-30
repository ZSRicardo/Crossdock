using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Crossdock.Context.Commands
{
    public class ConsultaZona : CrossDockContext
    {
        public Zonas consulta_zonas(string codigoPostal)//muestra un registro basado en el id
        {
            Zonas ozona=new Zonas();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base};convert zero datetime=True";
            List<Zonas> List = new List<Zonas>();
            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_zona_sp";
                // Parametros de SP
                cmd.Parameters.AddWithValue("codigopostal",codigoPostal);
                conexion.Open();
                var leer = cmd.ExecuteReader();
                try
                {
                    while (leer.Read())
                    {
                        List.Add(new Zonas()
                        {
                            /*Igualar las propiedades con los parametros del SP*/
                            /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                             Descripcion = leer["zon_descripcion"].ToString()
                        });
                    }
                    return List[0];
                }
                
                catch (Exception e)
                {
                    conexion.Close();//cierra conexion
                    leer.Close();//cierra lista
                    return ozona;
                }
              
         
            }
        }
    }
}