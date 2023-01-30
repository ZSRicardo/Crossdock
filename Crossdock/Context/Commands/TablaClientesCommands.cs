using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaClientesCommands : CrossDockContext
    {
        public void Alta_Clientes(Clientes clientes)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_clientes_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("cl_id", clientes.ClienteID);
                cmd.Parameters.AddWithValue("cl_razonsocial", clientes.RazonSocial);
                cmd.Parameters.AddWithValue("cl_contacto", clientes.Contacto);
                cmd.Parameters.AddWithValue("cl_telefono", clientes.Telefono);
                cmd.Parameters.AddWithValue("cl_email", clientes.Email);
                cmd.Parameters.AddWithValue("cl_calle", clientes.Calle);
                cmd.Parameters.AddWithValue("cl_numeroext", clientes.NumeroExt);
                cmd.Parameters.AddWithValue("cl_numeroint", clientes.NumeroInt);
                cmd.Parameters.AddWithValue("cl_colonia", clientes.Colonia);
                cmd.Parameters.AddWithValue("cl_codigopostal", clientes.CodigoPostal);
                cmd.Parameters.AddWithValue("ps_id", clientes.PreciosServiciosID);

                string caracteres = clientes.RazonSocial.Substring(0, 3).ToUpper();//extrae los 3 primeros digitos del nombre
                var prueba = Convert.ToString(DateTime.Now.DayOfYear);//dia juliano
                var ano = DateTime.Now.Year.ToString().Remove(0, 2);//año actual
                var identificador = prueba + ano + caracteres;
                cmd.Parameters.AddWithValue("cl_identificador", identificador);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }

        public List<Clientes> Muestra_Clientes()//muestra todos los clientes activos
        {
            List<Clientes> List = new List<Clientes>();
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_clientes_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Clientes()//llena la lista de datos
                    {
                        ClienteID=leer.GetInt32("cli_id"),
                        ClienteIdentificador=leer.GetString("cli_identificador"),
                        RazonSocial = leer["cli_razonsocial"].ToString(),
                        Contacto = leer["cli_contacto"].ToString(),
                        Telefono = Convert.ToInt64(leer["cli_telefono"].ToString()),
                        Email = leer["cli_email"].ToString(),
                        Calle = leer["cli_calle"].ToString(),
                        NumeroExt = leer["cli_numeroext"].ToString(),
                        NumeroInt = leer["cli_numeroint"].ToString(),
                        Colonia = leer["cli_colonia"].ToString(),
                        CodigoPostal = leer["cli_codigopostal"].ToString(),
                    });
                }

                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public List<Clientes> Muestra_ClientesMod(int id)//muestra un clientes basado en el id
        {
            List<Clientes> List = new List<Clientes>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_clientesmod_sp";
                // Parametros de SP
                cmd.Parameters.AddWithValue("cl_id", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Clientes()//llena la lista de datos
                    {
                        ClienteID=leer.GetInt32("cli_id"),
                        ClienteIdentificador = leer.GetString("cli_identificador"),
                        RazonSocial = leer["cli_razonsocial"].ToString(),
                        Contacto = leer["cli_contacto"].ToString(),
                        Telefono = Convert.ToInt64(leer["cli_telefono"].ToString()),
                        Email = leer["cli_email"].ToString(),
                        Calle = leer["cli_calle"].ToString(),
                        NumeroExt = leer["cli_numeroext"].ToString(),
                        NumeroInt = leer["cli_numeroint"].ToString(),
                        Colonia = leer["cli_colonia"].ToString(),
                        CodigoPostal = leer["cli_codigopostal"].ToString(),
                        PreciosServiciosID = Convert.ToInt32(leer["pse_id"].ToString()),
                    });
                }

                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public void Elimina_Clientes(int id)
        {

            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_clientes_sp";
                cmd.Parameters.AddWithValue("cl_id", id);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }
    }
}