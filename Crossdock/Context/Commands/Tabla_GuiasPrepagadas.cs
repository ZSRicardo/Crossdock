using Crossdock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class Tabla_GuiasPrepagadas : CrossDockContext
    {
        public int Alta_GuiasPrepagadas(GuiaPrepagada oguia)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_guiasprepagadas_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("cliid", oguia.ClienteID);
                cmd.Parameters.AddWithValue("numerodeguias", oguia.NumeroGuias);
                cmd.Parameters.AddWithValue("fechacompra", DateTime.Now);
             //   cmd.Parameters.AddWithValue("fechavencimiento", oguia.FechaVenciminto);


                // Cierre General
                conexion.Open();
                int IDdevuelto = 0;

             //   Lectura de datos
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDdevuelto = Convert.ToInt32(reader["gpr_id"]);
                    }
                }


                conexion.Close();
                return IDdevuelto;
            }
        }//Genera un nuevo registro si se ingresa el id en 0 si no modifica los datos


        public List<GuiaPrepagada> Muestra_GuiasPrepagadas()//muestra todos los anaqueles activos
        {
            List<GuiaPrepagada> List = new List<GuiaPrepagada>();
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_guiasprepagadas_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {

                    GuiaPrepagada oguia=new GuiaPrepagada();
                    oguia.ClienteID = leer.GetInt32("cli_id");
                    oguia.Cliente_RazonSocial = leer["cli_razonsocial"].ToString();
                    oguia.NumeroGuias = leer.GetInt32("gpr_numerodeguias");
                    if (leer["gpr_fechacompra"].ToString()=="")
                    {
                        oguia.FechaCompra = null;
                    }
                    else
                    {
                        oguia.FechaCompra = Convert.ToDateTime(leer["gpr_fechacompra"].ToString());
                    }
                    if (leer["gpr_fechavencimiento"].ToString() == "")
                    {
                        oguia.FechaVenciminto = null;
                    }
                    else
                    {
                        oguia.FechaVenciminto = Convert.ToDateTime(leer["gpr_fechavencimiento"].ToString());
                    }

                    List.Add(oguia);
                   
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public List<GuiaPrepagada> Muestra_GuiasPrepgadasMod(int gprID)//muestra un anaquel basado en el id
        {
            List<GuiaPrepagada> List = new List<GuiaPrepagada>();
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_guiasprepagadasmod_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("gprid", gprID);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    GuiaPrepagada oguia = new GuiaPrepagada();
                    oguia.ClienteID = leer.GetInt32("cli_id");
                    oguia.Cliente_RazonSocial = leer["cli_razonsocial"].ToString();
                    oguia.NumeroGuias = leer.GetInt32("gpr_numerodeguias");
                    if (leer["gpr_fechacompra"].ToString() == "")
                    {
                        oguia.FechaCompra = null;
                    }
                    else
                    {
                        oguia.FechaCompra = Convert.ToDateTime(leer["gpr_fechacompra"].ToString());
                    }
                    if (leer["gpr_fechavencimiento"].ToString() == "")
                    {
                        oguia.FechaVenciminto = null;
                    }
                    else
                    {
                        oguia.FechaVenciminto = Convert.ToDateTime(leer["gpr_fechavencimiento"].ToString());
                    }
                    List.Add(oguia);
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public void Elimina_GuiasPrepagadas(int gprID)
        {

            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_guiasprepagadas_sp";

                // Parametros de SP

                cmd.Parameters.AddWithValue("gprid", gprID);
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }

        }//borrado logico de un registro
    }
}