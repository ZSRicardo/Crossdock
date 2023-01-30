using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaHistorialSmsCommands : CrossDockContext
    {
        public void Alta_HistorialSms(DateTime hsmfecha, string hsmmensaje, int paqid, int desid, int usuid)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_historial_sms_sp";

                // Parametros de SP                
                cmd.Parameters.AddWithValue("hsmfecha", hsmfecha);
                cmd.Parameters.AddWithValue("hsmmensaje", hsmmensaje);
                cmd.Parameters.AddWithValue("paqid", paqid);
                cmd.Parameters.AddWithValue("desid", desid);
                cmd.Parameters.AddWithValue("usuid", usuid);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }//Genera un nuevo registro si se ingresa el id en 0 si no modifica los datos
        public List<HistorialSms> Muestra_HistorialSms()//muestra todos los registros activos
        {
            List<HistorialSms> List = new List<HistorialSms>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_historial_sms_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new HistorialSms()//llena la lista de datos
                    {
                        HistorialSmsID=leer.GetInt32("hsm_id"),
                        Fecha=Convert.ToDateTime(leer["hsm_fecha"].ToString()),
                        Mensaje= leer["hsm_mensaje"].ToString(),
                        Guia = leer["paq_guia"].ToString(),
                        DestinatarioNombre= leer["des_nombre"].ToString(),
                        DestinatarioApellidoP= leer["des_apellidop"].ToString(),
                        DestinatarioApellidoM= leer["des_apellidom"].ToString(),
                        UsuarioNombre= leer["usu_nombre"].ToString(),


                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }
    }
}