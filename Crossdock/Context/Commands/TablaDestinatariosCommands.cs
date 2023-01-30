using Crossdock.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaDestinatariosCommands : CrossDockContext
    {

        public void Alta_Destinatarios(Destinatarios destinatarios)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {

                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_destinatarios_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("de_id", destinatarios.DestinatarioID);
                cmd.Parameters.AddWithValue("de_nombre", destinatarios.Nombre);
                cmd.Parameters.AddWithValue("de_apellidop", destinatarios.ApellidoP);
                cmd.Parameters.AddWithValue("de_apellidom", destinatarios.ApellidoM);
                cmd.Parameters.AddWithValue("de_celular", destinatarios.Celular);
                cmd.Parameters.AddWithValue("de_email", destinatarios.Email);
                cmd.Parameters.AddWithValue("de_calle", destinatarios.Calle);
                cmd.Parameters.AddWithValue("de_numeroext", destinatarios.NumeroExt);
                cmd.Parameters.AddWithValue("de_numeroint", destinatarios.NumeroInt);
                cmd.Parameters.AddWithValue("de_colonia", destinatarios.Colonia);
                cmd.Parameters.AddWithValue("de_codigopostal", destinatarios.CodigoPostal);
                cmd.Parameters.AddWithValue("de_latitud", destinatarios.Latitud);
                cmd.Parameters.AddWithValue("de_longitud", destinatarios.Longitud);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }

        public List<Destinatarios> Muestra_Destinatarios()//muestra todos los registros activos
        {
            List<Destinatarios> List = new List<Destinatarios>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_destinatarios_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Destinatarios()//llena la lista de datos
                    {
                        DestinatarioID = leer.GetInt32("des_id"),
                        Nombre = leer["des_nombre"].ToString(),
                        ApellidoP = leer["des_apellidop"].ToString(),
                        ApellidoM = leer["des_apellidom"].ToString(),
                        Celular = (long)leer["des_celular"],
                        Email = leer["des_email"].ToString(),
                        Calle = leer["des_calle"].ToString(),
                        NumeroExt = leer["des_numeroext"].ToString(),
                        NumeroInt = leer["des_numeroint"].ToString(),
                        Colonia = leer["des_colonia"].ToString(),
                        CodigoPostal = leer["des_codigopostal"].ToString(),
                        Latitud = (double)leer["des_latitud"],
                        Longitud = (double)leer["des_longitud"],

                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }
        public List<Destinatarios> Muestra_DestinatariosMod(Destinatarios destinatarios)//muestra un registro basado en el id
        {
            List<Destinatarios> List = new List<Destinatarios>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {


                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_destinatariosmod_sp";
                // Parametros de SP

                cmd.Parameters.AddWithValue("de_id", destinatarios.DestinatarioID);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Destinatarios()//llena la lista de datos
                    {
                        DestinatarioID=leer.GetInt32("des_id"),
                        Nombre = leer["des_nombre"].ToString(),
                        ApellidoP = leer["des_apellidop"].ToString(),
                        ApellidoM = leer["des_apellidom"].ToString(),
                        Celular = (long)leer["des_celular"],
                        Email = leer["des_email"].ToString(),
                        Calle = leer["des_calle"].ToString(),
                        NumeroExt = leer["des_numeroext"].ToString(),
                        NumeroInt = leer["des_numeroint"].ToString(),
                        Colonia = leer["des_colonia"].ToString(),
                        CodigoPostal = leer["des_codigopostal"].ToString(),
                        Latitud = (double)leer["des_latitud"],
                        Longitud = (double)leer["des_longitud"],
                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public void Elimina_Destinatarios(Destinatarios destinatarios)
        {

            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {


                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_destinatarios_sp";
                cmd.Parameters.AddWithValue("de_id", destinatarios.DestinatarioID);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }
    }
}