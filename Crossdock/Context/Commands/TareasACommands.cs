using Crossdock.Models;
using DocumentFormat.OpenXml.Bibliography;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Crossdock.Context.Commands
{

    public class TareasACommands : CrossDockContext
    {
        public void Alta_Tarea(Tareas tareas)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base};convert zero datetime=True";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_tarea_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("tarid", tareas.Tarea_ID);
                cmd.Parameters.AddWithValue("paqguia", tareas.Guia);
                cmd.Parameters.AddWithValue("tarfecha", tareas.Fecha_Asigacion);
                cmd.Parameters.AddWithValue("etaid", tareas.Estatus);
                cmd.Parameters.AddWithValue("tarintento", tareas.Intento);
                cmd.Parameters.AddWithValue("tarcomentarios", tareas.Comentarios);
                cmd.Parameters.AddWithValue("usuid", tareas.Usuario_ID);
                cmd.Parameters.AddWithValue("ttaid", tareas.Tipo_Tarea);
                cmd.Parameters.AddWithValue("tarfechafin", null);
                cmd.Parameters.AddWithValue("tarlatitud", null);
                cmd.Parameters.AddWithValue("tarlongitud", null);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }

        public List<Tareas> consultaTareas(int usu_id)//muestra un registro basado en el id
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base};convert zero datetime=True";
            List<Tareas> L_Tareas = new List<Tareas>();
            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {

                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_tareas_sp";//modificar nombre del delegaciobn
                // Parametros de SP
                cmd.Parameters.AddWithValue("usuid", usu_id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                try
                {
                    while (leer.Read())
                    {
                        L_Tareas.Add(new Tareas()//llena la lista de datos
                        {

                            Tarea_ID = leer.GetInt32("tar_id"),
                            Intento = leer.GetInt32("tar_intento"),
                            Fecha_Asigacion = (DateTime)leer["tar_fecha"],
                            Guia = leer.GetInt32("paq_guia").ToString(),
                            Estatus = leer["eta_descripcion"].ToString(),
                            Usuario_ID = leer.GetInt32("usu_id"),
                            Usuario_Identificador = leer["usu_identificador"].ToString(),
                            Celular = leer["ope_celular"].ToString(),
                        }); ;
                    }


                    conexion.Close();//cierra conexion
                    leer.Close();
                    return L_Tareas;
                }
                catch (Exception e)
                {
                    conexion.Close();//cierra conexion
                    leer.Close();
                    return L_Tareas;
                }


            }
        }

        public List<Tareas> Muestra_Tareas()//muestra la lista de tareas
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base};convert zero datetime=True";
            List<Tareas> L_Tareas = new List<Tareas>();
            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {

                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_tareasmod_sp";//modificar nombre del delegaciobn
                // Parametros de SP
                conexion.Open();
                var leer = cmd.ExecuteReader();

                try
                {
                    while (leer.Read())
                    {
                        L_Tareas.Add(new Tareas()//llena la lista de datos
                        {

                            Tarea_ID = leer.GetInt32("tar_id"),
                            Intento = leer.GetInt32("tar_intento"),
                            Fecha_Asigacion = (DateTime)leer["tar_fecha"],
                            Guia = leer["paq_guia"].ToString(),
                            Estatus = leer["eta_descripcion"].ToString(),
                            Usuario_ID = leer.GetInt32("usu_id"),
                            Usuario_Nombre = leer["usu_nombre"].ToString(),
                            Celular = leer["ope_celular"].ToString(),
                            Zona_ID = leer.GetInt32("zon_id"),
                        }); ;
                    }
                    conexion.Close();//cierra conexion
                    leer.Close();
                    return L_Tareas;
                }
                catch (Exception e)
                {
                    conexion.Close();//cierra conexion
                    leer.Close();
                    return L_Tareas;
                }


            }
        }

        public List<Tareas> Tareas_Generales()//muestra la lista de tareas generales
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base};convert zero datetime=True";
            List<Tareas> L_Tareas = new List<Tareas>();
            // Utiliza dispose al finalizar bloque
            //creamos una instancia de TablaUsuariosCommands
            TablaUsuariosCommands oUsuario = new TablaUsuariosCommands();
            TablaGuiasCommands oGuia = new TablaGuiasCommands();
            int contador = 0;
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {

                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_tareasgenerales_sp";//modificar nombre del delegaciobn
                // Parametros de SP
                conexion.Open();
                var leer = cmd.ExecuteReader();

                try
                {
                    while (leer.Read())
                    {

                        Tareas prueba = new Tareas();
                        prueba.Tipo_Tarea_Descripcion = leer["tta_descripcion"].ToString();
                        prueba.Nombre_Destinatario = leer["nombre_destinatario"].ToString();
                        prueba.Direccion_Destinatario = leer["direccion_destinatario"].ToString();
                        prueba.Tarea_ID = leer.GetInt32("tar_id");
                        prueba.Palabra_Clave = leer["paq_codigorecoleccion"].ToString();
                        prueba.C_Razon_Social = leer["cli_razonsocial"].ToString();
                        prueba.Intento = leer.GetInt32("tar_intento");
                        prueba.Guia = leer["paq_guia"].ToString();
                        prueba.Fecha_Asigacion = leer["tar_fecha"].ToString() is "" ? Convert.ToDateTime(null) : Convert.ToDateTime(leer["tar_fecha"].ToString());
                        prueba.Estatus = leer["eta_descripcion"].ToString();
                        prueba.Usuario_ID = leer["usu_id"].ToString() is "" ? 0 : Convert.ToInt32(leer["usu_id"]);
                        prueba.Celular = leer["ope_celular"].ToString();
                        prueba.Des_Zona = leer["zon_descripcion"].ToString();
                        prueba.Usuario_Nombre = leer["usu_nombre"].ToString();
                        prueba.Comentarios = leer["tar_comentarios"].ToString();
                        prueba.Evidencia = leer["tar_evidencia"].ToString();
                        prueba.Tarea_Activo = leer["tar_Activo"].ToString() is null ? "" : leer["tar_Activo"].ToString();
                        prueba.StringFecha_Aceptada = leer["tar_fechaaceptada"].ToString();
                        prueba.StringFecha_Fin = leer["tar_fechafin"].ToString();
                        prueba.Latitud = leer["tar_latitud"].ToString();
                        prueba.longitud = leer["tar_longitud"].ToString();

                        if (leer["tar_fecha"].ToString() == "")
                        {
                            prueba.fechaAsignacion = null;
                        }
                        else
                        {
                            prueba.fechaAsignacion = leer["tar_fecha"].ToString();
                        }


                        if (leer["tar_fechaaceptada"].ToString() == "")
                        {
                            prueba.Fecha_Aceptada = null;
                        }
                        else
                        {
                            prueba.Fecha_Aceptada = Convert.ToDateTime(leer["tar_fechaaceptada"].ToString());
                        }

                        if (leer["tar_fechafin"].ToString() == "")
                        {
                            prueba.Fecha_Fin = null;
                        }
                        else
                        {
                            prueba.Fecha_Fin = Convert.ToDateTime(leer["tar_fechafin"].ToString());
                        }

                        L_Tareas.Add(prueba);
                    }


                    conexion.Close();//cierra conexion
                    leer.Close();
                    return L_Tareas;
                }
                catch (Exception e)
                {
                    conexion.Close();//cierra conexion
                    leer.Close();
                    return L_Tareas;
                }


            }
        }

        public List<Tareas> TareasAsignadas_Fecha(string ofecha, string nc)//muestra un registro basado en el id
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base};convert zero datetime=True";
            List<Tareas> L_Tareas = new List<Tareas>();

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {

                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_tareas_fecha";//modificar nombre del delegacion
                // Parametros de SP

                cmd.Parameters.Add("?fechaasignacion", MySqlDbType.DateTime).Value = ofecha;
                cmd.Parameters.AddWithValue("usucelular", nc);
                // cmd.Parameters.AddWithValue("fechaasignacion", value: fecha);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                try
                {
                    while (leer.Read())
                    {
                        L_Tareas.Add(new Tareas()//llena la lista de datos
                        {

                            Tarea_ID = leer.GetInt32("tar_id"),
                            Intento = leer.GetInt32("tar_intento"),
                            Guia = leer["paq_guia"].ToString(),
                            Estatus = leer["eta_descripcion"].ToString(),
                            // Usuario_ID = leer.GetInt32("usu_id"),
                            Usuario_Identificador = leer["usu_identificador"].ToString(),
                            Celular = leer["ope_celular"].ToString(),
                            Tipo_Tarea_Descripcion = leer["tta_descripcion"].ToString(),
                            Nombre_Destinatario = leer["Nombre_Destinatario"].ToString(),
                            Direccion_Destinatario = leer["Direccion_Destinatario"].ToString(),
                            Des_Zona = leer["zon_descripcion"].ToString(),
                            C_Razon_Social = leer["cli_razonsocial"].ToString(),
                            Tarea_Activo = leer["tar_Activo"].ToString() is null ? "" : leer["tar_Activo"].ToString(),
                            Palabra_Clave = leer["paq_codigorecoleccion"].ToString()


                        });
                    }


                    conexion.Close();//cierra conexion
                    leer.Close();
                    return L_Tareas;
                }
                catch (Exception e)
                {
                    conexion.Close();//cierra conexion
                    leer.Close();
                    return L_Tareas;
                }


            }
        }


        public void Elimina_Tarea(string guia, DateTime fecha)
        {

            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_tarea_sp";
                cmd.Parameters.AddWithValue("paqguia", guia);
                cmd.Parameters.AddWithValue("tarfecha", fecha);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }




    }
}