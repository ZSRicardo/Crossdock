using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Queries
{
    public class TablaGuiasQueries : CrossDockContext
    {
        public List<Guias> Muestra_Guias()//muestra todos los registros activos
        {
            List<Guias> List = new List<Guias>();
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";

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
                    List.Add(new Guias()//llena la lista de datos
                    {
                        Guia = leer["gu_guia"].ToString(),
                        FechaCreacion = Convert.ToDateTime(leer["gu_fechacreacion"].ToString()),
                        Medida = leer["gu_medida"].ToString(),
                        Peso = Convert.ToDouble(leer["gu_peso"]),
                        Descripcion = leer["gu_descripcion"].ToString(),
                        Instrucciones = leer["gu_instrucciones"].ToString(),
                        Url = leer["gu_url"].ToString(),
                        DestinatarioID = Convert.ToInt32(leer["de_id"].ToString()),
                        ClienteID = Convert.ToInt32(leer["cl_id"].ToString()),
                        UsuarioID = Convert.ToInt32(leer["us_id"].ToString()),
                        DeliveryID = Convert.ToInt32(leer["dell_id"].ToString()),
                        ZonaId= Convert.ToInt32(leer["zon_id"].ToString()),
                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista

                return List;//regresa la lista con datos
            }
        }

        // Utiliza un viewmodel adaptado al join que realiza el SP
        public DatosGuiaViewModel Muestra_Guia(string guiaInput)
        {
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_guiasmod_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("guiguia", guiaInput);

                using (var leer = cmd.ExecuteReader())
                {
                    DatosGuiaViewModel datosGuia = new DatosGuiaViewModel();

                    while (leer.Read())
                    {
                        datosGuia.GuiaID = Convert.ToInt32(leer["gui_id"]);
                        datosGuia.Guia = leer["gui_guia"].ToString();
                        datosGuia.FechaCreacion = Convert.ToDateTime(leer["gui_fechacreacion"]);
                        datosGuia.Medida = leer["gui_medida"].ToString();
                        datosGuia.Peso = Convert.ToDouble(leer["gui_peso"]);
                        datosGuia.Descripcion = leer["gui_descripcion"].ToString();
                        datosGuia.DestinatarioNombre = leer["Nombre_Destinatario"].ToString();
                        datosGuia.DestinatarioApellidoP = leer["des_apellidop"].ToString();
                        datosGuia.DestinatarioApellidoM = leer["des_apellidom"].ToString();
                        datosGuia.DestinatarioCelular = Convert.ToInt64(leer["des_celular"]);
                        datosGuia.DestinatarioEmail = leer["des_email"].ToString();
                        datosGuia.DestinatarioCalle = leer["des_calle"].ToString();
                        datosGuia.DestinatarioNumeroExt = leer["des_numeroext"].ToString();
                        datosGuia.DestinatarioNumeroInt = leer["des_numeroint"].ToString();
                        datosGuia.DestinatarioDireccion = leer["Direccion_Destinatario"].ToString();
                        datosGuia.DestinatarioCodigoPostal = leer["des_codigopostal"].ToString();
                        datosGuia.Cliente = leer["cli_razonsocial"].ToString();
                        datosGuia.Zona = leer["zon_descripcion"].ToString();

                        if (leer["paq_id"].ToString() == "" || leer["paq_id"].ToString() == null)
                        {
                            datosGuia.paqID = null;
                        }
                        else
                        {
                            datosGuia.paqID = leer["paq_id"].ToString();
                        }

                        if (leer["epa_id"].ToString() == "" || leer["epa_id"].ToString() == null)
                        {
                            datosGuia.EstatusPaqueteID = null;
                        }
                        else
                        {
                            datosGuia.EstatusPaqueteID = leer["epa_id"].ToString();
                        }

                        if (leer["paq_intento"].ToString() == "" || leer["paq_intento"].ToString() == null)
                        {
                            datosGuia.NumeroIntento = null;
                        }
                        else
                        {
                            datosGuia.NumeroIntento = leer["paq_intento"].ToString();
                        }

                        if (leer["zon_id"].ToString() == "" || leer["zon_id"].ToString() == null || Convert.ToInt32(leer["zon_id"].ToString()) == 0)
                        {
                            datosGuia.Zonaid = 11;
                        }
                        else
                        {
                            datosGuia.Zonaid = Convert.ToInt32(leer["zon_id"].ToString());
                        }

                    }

                    return datosGuia; //regresa objeto con datos
                }
            }
        }
    }
}