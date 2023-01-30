using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaGuiasCommands : CrossDockContext
    {
        public List<Guias> Muestra_Guias()//muestra todos los registros activos
        {
            List<Guias> List = new List<Guias>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {

                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_guias_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    Guias g = new Guias();
                    //  g.Instrucciones = leer["gui_instrucciones"].ToString();
                    g.GuiaID = leer.GetInt32("gui_id");
                    g.Guia = leer["gui_guia"].ToString();
                    g.FechaCreacion = Convert.ToDateTime(leer["gui_fechacreacion"].ToString());
                    g.Medida = leer["gui_medida"].ToString();
                    g.Peso = Convert.ToDouble(leer["gui_peso"].ToString());
                    g.Descripcion = leer["gui_descripcion"].ToString();
                    g.Url = leer["gui_url"].ToString();
                    g.Instrucciones = leer["gui_instrucciones"].ToString();
                    g.Destinatario = leer["Nombre de Destinatario"].ToString();
                    g.DireccionDestinatario = leer["Direccion de Destinatario"].ToString();
                    g.Cliente_RZ = leer["cli_razonsocial"].ToString();
                    g.ZonaDes = leer["zon_descripcion"].ToString();
                    g.Fecha = leer["gui_fechacreacion"].ToString();
                    g.ClienteID = Convert.ToInt32(leer["cli_id"]);
                    List.Add(g);
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public List<Guias> Muestra_GuiasMod(string guia)//muestra todos los registros activos
        {
            List<Guias> lGUias = new List<Guias>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_guiasmod_sp";
                //parametros
                cmd.Parameters.AddWithValue("guiguia", guia);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    Guias g = new Guias();
                    //  g.Instrucciones = leer["gui_instrucciones"].ToString();
                    g.GuiaID = leer.GetInt32("gui_id");
                    g.Guia = leer["gui_guia"].ToString();
                    g.FechaCreacion = Convert.ToDateTime(leer["gui_fechacreacion"].ToString());
                    g.Medida = leer["gui_medida"].ToString();
                    g.Peso = Convert.ToDouble(leer["gui_peso"].ToString());
                    g.Descripcion = leer["gui_descripcion"].ToString();
                    g.Url = leer["gui_url"].ToString();
                    g.Destinatario = leer["Nombre_Destinatario"].ToString();
                    g.DireccionDestinatario = leer["Direccion_Destinatario"].ToString();
                    g.Cliente_RZ = leer["cli_razonsocial"].ToString();
                    g.ZonaDes = leer["zon_descripcion"].ToString();
                    g.Fecha = Convert.ToDateTime(leer["gui_fechacreacion"].ToString()).ToString("dddd dd 'de' MMMM 'de' yyyy");
                    g.ClienteID = Convert.ToInt32(leer["cli_id"]);
                   
                    lGUias.Add(g);
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return lGUias;//regresa la lista con datos
            }
        }


        public void Elimina_Guia(int id)
        {
            string connectionString = $"server = {GetRDSConections().Writer}; {Data_base}";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_guias_sp";
                cmd.Parameters.AddWithValue("guiid", id);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }


        }

    }
}