using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaDeliveryCommands : CrossDockContext
    {
        public void Alta_Delivery(Delivery delivery)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_delivery_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("de_id", delivery.DeliveryID);
                cmd.Parameters.AddWithValue("de_nombre", delivery.Nombre);
                cmd.Parameters.AddWithValue("de_telefono", delivery.Telefono);
                cmd.Parameters.AddWithValue("de_calle", delivery.Calle);
                cmd.Parameters.AddWithValue("de_numeroext", delivery.NumeroExt);
                cmd.Parameters.AddWithValue("de_numeroint", delivery.NumeroInt);
                cmd.Parameters.AddWithValue("de_colonia", delivery.Colonia);
                cmd.Parameters.AddWithValue("de_codigopostal", delivery.CodigoPostal);
                cmd.Parameters.AddWithValue("ps_id", delivery.PreciosServiciosID);

                string caracteres = delivery.Nombre.Substring(0, 3).ToUpper();//extrae los 3 primeros digitos del nombre
                var prueba = Convert.ToString(DateTime.Now.DayOfYear);//dia juliano
                var ano = DateTime.Now.Year.ToString().Remove(0, 2);//año actual
                var identificador = prueba + ano + caracteres;
                cmd.Parameters.AddWithValue("de_identificador", identificador);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }//Genera un neuvo registro si se ingresa el id en 0 si no modifica los datos

        public List<Delivery> Muestra_Delivery()//muestra todos los delivery activos
        {
            List<Delivery> List = new List<Delivery>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_delivery_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Delivery()//llena la lista de datos
                    {
                        DeliveryID = leer.GetInt32("del_id"),
                        DeliveryIdentificador = leer.GetString("del_identificador"),
                        Nombre = leer["del_nombre"].ToString(),
                        Telefono = Convert.ToInt64(leer["del_telefono"].ToString()),
                        Calle = leer["del_calle"].ToString(),
                        NumeroExt = leer["del_numeroext"].ToString(),
                        NumeroInt = leer["del_numeroint"].ToString(),
                        Colonia = leer["del_colonia"].ToString(),
                        CodigoPostal = leer["del_codigopostal"].ToString(),
                        PreciosServiciosID = leer["pse_id"].ToString(),
                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public List<Delivery> Muestra_DeliveryMod(int id)//muestra un delivery  basado en el id
        {
            List<Delivery> List = new List<Delivery>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_deliverymod_sp";
                // Parametros de SP
                cmd.Parameters.AddWithValue("de_id", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Delivery()//llena la lista de datos
                    {
                        DeliveryID = leer.GetInt32("del_id"),
                        DeliveryIdentificador = leer.GetString("del_identificador"),
                        Nombre = leer["del_nombre"].ToString(),
                        Telefono = Convert.ToInt64(leer["del_telefono"].ToString()),
                        Calle = leer["del_calle"].ToString(),
                        NumeroExt = leer["del_numeroext"].ToString(),
                        NumeroInt = leer["del_numeroint"].ToString(),
                        Colonia = leer["del_colonia"].ToString(),
                        CodigoPostal = leer["del_codigopostal"].ToString(),
                        PreciosServiciosID = leer["pse_id"].ToString(),
                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public void Elimina_Delivery(int id)
        {

            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_delivery_sp";
                cmd.Parameters.AddWithValue("de_id", id);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }
    }
}