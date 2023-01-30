using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaCoberturasCommands : CrossDockContext
    {
        public void Alta_Coberturas(Coberturas coberturas)
        {
            string connectionString = $"server = {GetRDSConections().Writer}; {Data_base}";

            //Utiliza dispose al finalizar el bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_coberturas_sp";

                //Parametros sp
                cmd.Parameters.AddWithValue("cobid", coberturas.CoberturaID);
                cmd.Parameters.AddWithValue("cobcolonia", coberturas.Colonia);
                cmd.Parameters.AddWithValue("cobcodigopostal", coberturas.CodigoPostal);
                cmd.Parameters.AddWithValue("delid", coberturas.DeliveryID);
                cmd.Parameters.AddWithValue("zonid", coberturas.ZonaID);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }

        public List<Coberturas> Muestra_Coberturas()
        {
            List<Coberturas> List = new List<Coberturas>(); 
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";

            //Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos sp
                MySqlCommand cmd = new MySqlCommand()
                {

                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "muestra_coberturas_sp"
                };
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    //Llena lista de datos
                    List.Add(new Coberturas()
                    {
                        CoberturaID = leer.GetInt32("cob_id"),
                        Colonia = leer.GetString("cob_colonia"),
                        CodigoPostal = leer["cob_codigopostal"].ToString(),
                        ZonaDescripcion = leer["zon_descripcion"].ToString(),
                        DeliveryNombre = leer["del_nombre"].ToString(),
                        DeliveryTelefono = leer["del_telefono"].ToString(),
                    }); 
                }

                conexion.Close();//Cierra la conexión
                leer.Close();//Cierra la lista
                return List;//regresa lista con datos
            }
        }

        public List<Coberturas> Muestra_CoberturasMod(int id)
        {
            List<Coberturas> List = new List<Coberturas>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            //Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_coberturasmod_sp";
                // Parametros de SP
                cmd.Parameters.AddWithValue("cobid", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    //Llena lista de datos
                    List.Add(new Coberturas()
                    {
                        CoberturaID = leer.GetInt32("cob_id"),
                        Colonia = leer.GetString("cob_colonia"),
                        CodigoPostal = leer.GetString("cob_codigopostal"),
                        DeliveryID = Convert.ToInt32(leer["del_id"].ToString()),
                        ZonaID = Convert.ToInt32(leer["zon_id"].ToString()),
                    }); 
                }

                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public void Elimina_Coberturas(int id)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_coberturas_sp";
                cmd.Parameters.AddWithValue("cobid", id);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }
    }
}