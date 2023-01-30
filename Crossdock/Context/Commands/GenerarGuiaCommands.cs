using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class GenerarGuiaCommands : CrossDockContext
    {

        public int AltaDestinatario(Destinatarios destinatario)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_destinatarios_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("de_id", destinatario.DestinatarioID);
                cmd.Parameters.AddWithValue("de_nombre", destinatario.Nombre);
                cmd.Parameters.AddWithValue("de_apellidop", destinatario.ApellidoP);
                cmd.Parameters.AddWithValue("de_apellidom", destinatario.ApellidoM);
                cmd.Parameters.AddWithValue("de_celular", destinatario.Celular);
                cmd.Parameters.AddWithValue("de_email", destinatario.Email);
                cmd.Parameters.AddWithValue("de_calle", destinatario.Calle);
                cmd.Parameters.AddWithValue("de_numeroext", destinatario.NumeroExt);
                cmd.Parameters.AddWithValue("de_numeroint", destinatario.NumeroInt);
                cmd.Parameters.AddWithValue("de_colonia", destinatario.Colonia);
                cmd.Parameters.AddWithValue("de_codigopostal", destinatario.CodigoPostal);
                cmd.Parameters.AddWithValue("de_latitud", destinatario.Latitud);
                cmd.Parameters.AddWithValue("de_longitud", destinatario.Longitud);

                int IDdevuelto = 0;

                // Lectura de datos
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDdevuelto = Convert.ToInt32(reader["_destinatarioId"]);
                    }
                }

                return IDdevuelto;
            }
        }

        public int VerificaDestinatario(Destinatarios destinatario)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "verifica_destinatario_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("de_nombre", destinatario.Nombre);
                cmd.Parameters.AddWithValue("de_apellidop", destinatario.ApellidoP);
                cmd.Parameters.AddWithValue("de_apellidom", destinatario.ApellidoM);
                cmd.Parameters.AddWithValue("de_celular", destinatario.Celular);
                cmd.Parameters.AddWithValue("de_email", destinatario.Email);
                cmd.Parameters.AddWithValue("de_calle", destinatario.Calle);
                cmd.Parameters.AddWithValue("de_numeroext", destinatario.NumeroExt);
                cmd.Parameters.AddWithValue("de_numeroint", destinatario.NumeroInt);
                cmd.Parameters.AddWithValue("de_colonia", destinatario.Colonia);
                cmd.Parameters.AddWithValue("de_codigopostal", destinatario.CodigoPostal);
                cmd.Parameters.AddWithValue("de_latitud", destinatario.Latitud);
                cmd.Parameters.AddWithValue("de_longitud", destinatario.Longitud);

                int IDdevuelto = 0;

                // Lectura de datos
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDdevuelto = Convert.ToInt32(reader["_destinatarioId"]);
                    }
                }

                return IDdevuelto;
            }
        }




        public void AltaGuia(Guias guiaTaimingo)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                

                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_guias_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("gu_identificador",guiaTaimingo.NoPedido);
                cmd.Parameters.AddWithValue("gu_id", guiaTaimingo.GuiaID);
                cmd.Parameters.AddWithValue("gu_guia", guiaTaimingo.Guia);
                cmd.Parameters.AddWithValue("gu_fechacreacion", guiaTaimingo.FechaCreacion);
                cmd.Parameters.AddWithValue("gu_medida", guiaTaimingo.Medida);
                cmd.Parameters.AddWithValue("gu_peso", guiaTaimingo.Peso);
                cmd.Parameters.AddWithValue("gu_descripcion", guiaTaimingo.Descripcion);
                cmd.Parameters.AddWithValue("gu_instrucciones", guiaTaimingo.Instrucciones);
                cmd.Parameters.AddWithValue("gu_url", guiaTaimingo.Url);
                cmd.Parameters.AddWithValue("de_id", guiaTaimingo.DestinatarioID);
                cmd.Parameters.AddWithValue("cl_id", guiaTaimingo.ClienteID);
                cmd.Parameters.AddWithValue("us_id", guiaTaimingo.UsuarioID);
                cmd.Parameters.AddWithValue("dell_id", guiaTaimingo.DeliveryID);
                cmd.Parameters.AddWithValue("gu_recoleccion", guiaTaimingo.Tipo_Guia);


                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;

            }
        }


    } // Fin de clase
}