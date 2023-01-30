using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class BusquedaGuia : CrossDockContext
    {
        public List<SeguimientoPaqs> Busqueda_Guia(string inputGuia)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base};convert zero datetime=True";

            List<SeguimientoPaqs> list = new List<SeguimientoPaqs>();

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "seguimiento_paquetes_sp";
                //parametros
                cmd.Parameters.AddWithValue("paqguia", inputGuia);
                conexion.Open();  

                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    SeguimientoPaqs seguimientoPaqs = new SeguimientoPaqs();


                    seguimientoPaqs.FechaRecepcion = leer["paq_fecharecepcion"].ToString().Substring(0, 9);
                   
                   
                    seguimientoPaqs.Estatus = leer["epa_descripcion"].ToString();
                    if (leer["paq_fechaentregafinal"].ToString() == "")
                    {
                        seguimientoPaqs.FechaEntrega = null;
                    }
                    else
                    {
                        seguimientoPaqs.FechaEntrega = leer["paq_fechaentregafinal"].ToString().Substring(0, 9);
                        
                    }

                    seguimientoPaqs.EvidenciaEntrega = leer["tar_evidencia"].ToString();

                    list.Add(seguimientoPaqs);
                }

                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
            }
            return list;//regresa la lista con datos
        }
    }
}