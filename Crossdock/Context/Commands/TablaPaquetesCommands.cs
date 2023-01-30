using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaPaquetesCommands : CrossDockContext
    {
        /// <summary>
        /// Da de alta un nuevo registro Paquetes en la base de datos. Genera un nuevo registro si se ingresa el id en "0", si no, modifica el registro existente.
        /// </summary>
        public void Alta_Paquetes(Paquetes Paquete)
        {
            //Conexión a la base de datos //Writer porque Altas son escrituras
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            ConsultaZona oZona = new ConsultaZona();
            TablaZonasCommands oLZona = new TablaZonasCommands();
            var listaZonas = oLZona.Muestra_Zonas();

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Alta
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_paquetes_sp";

                /*Pasar las propiedades a los parametros del SP*/
                /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                /*cmd.Parameters.AddWithValue("(nombre del parametro en el SP)", OBJECT.PARAMETER);*/

                cmd.Parameters.AddWithValue("pa_id", 0);
                cmd.Parameters.AddWithValue("pa_fecharecepcion", Paquete.FechaRecepcion);
                cmd.Parameters.AddWithValue("pa_guia", Paquete.Guia);
                cmd.Parameters.AddWithValue("pa_codigorecoleccion", Paquete.CodigoRecoleccion);
                cmd.Parameters.AddWithValue("pa_fechaingresobodega", Paquete.FechaRecepcion);
                cmd.Parameters.AddWithValue("pa_fechasalidabodega", Paquete.FechaSalidaBodega);
                cmd.Parameters.AddWithValue("pa_fechacargaoperador", Paquete.FechaCargaOperador);
                cmd.Parameters.AddWithValue("pa_fechaentregafinal", Paquete.FechaEntregaFinal);
                cmd.Parameters.AddWithValue("pa_gps", Paquete.GPS);
                cmd.Parameters.AddWithValue("pa_intento", Paquete.Intento);
                cmd.Parameters.AddWithValue("pa_foto", Paquete.Foto);
                if (Paquete.OperadorID == 0)
                {
                    cmd.Parameters.AddWithValue("op_id", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("op_id", Paquete.OperadorID);
                }
                cmd.Parameters.AddWithValue("us_id", Paquete.UsuarioID);
                cmd.Parameters.AddWithValue("ep_id", Paquete.EstatusPaqueteID);
                cmd.Parameters.AddWithValue("bo_id", null);
                cmd.Parameters.AddWithValue("co_id", 1);
                cmd.Parameters.AddWithValue("zo_id", Paquete.ZonaID);
                if (Paquete.AnaquelID == 0)
                {

                    cmd.Parameters.AddWithValue("an_id", null);

                }
                else
                {
                    cmd.Parameters.AddWithValue("an_id", Paquete.AnaquelID);
                }
                cmd.Parameters.AddWithValue("gu_id", Paquete.GuiID);
                if (Paquete.MotivoDevolucionID == 0)
                {
                    cmd.Parameters.AddWithValue("de_id", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("de_id", Paquete.MotivoDevolucionID);
                }

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }


        public void desactivaPaquete(int id)
        {
            //Conexión a la base de datos //Writer porque Altas son escrituras
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Alta
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "desactiva_paquete_sp";

                /*Pasar las propiedades a los parametros del SP*/
                /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                /*cmd.Parameters.AddWithValue("(nombre del parametro en el SP)", OBJECT.PARAMETER);*/

                cmd.Parameters.AddWithValue("paqid", id);
                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }



        public List<Paquetes> Muestra_Paquetes_Para_Asignacion()//muestra todos los registros activos
        {
            List<Paquetes> List = new List<Paquetes>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base};convert zero datetime=True";
            TareasACommands oTareas = new TareasACommands();
            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_paquetes_para_asignacion_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Paquetes()//llena la lista de datos
                    {
                        Guia = leer["paq_guia"].ToString(),
                        Intento = leer.GetInt32("paq_intento"),
                        Zona = leer["zon_descripcion"].ToString(),
                        FechaIngresoBodega = leer["paq_fechaingresobodega"].ToString(),
                        FechaEntregaFinal = leer["paq_fechaentregafinal"].ToString(),
                        EstatusPaqueteDescripcion = leer["epa_descripcion"].ToString(),
                        Razon_Social_Cliente = leer["cli_razonsocial"].ToString()

                    });
                }

                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public List<Guias> Muestra_Paquetes_Por_Llegar()//muestra todos los registros activos
        {
            List<Guias> List = new List<Guias>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base};convert zero datetime=True";
            TareasACommands oTareas = new TareasACommands();
            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_paquetes_por_llegar_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Guias()//llena la lista de datos
                    {
                        Guia = leer["gui_guia"].ToString(),
                        Destinatario = leer["Nombre de Destinatario"].ToString(),
                        DireccionDestinatario = leer["Direccion de Destinatario"].ToString(),
                        Cliente_RZ = leer["cli_razonsocial"].ToString(),
                        FechaCreacion = leer.GetDateTime("gui_fechacreacion"),
                        ZonaDes = leer["zon_descripcion"].ToString()
                       
                    });
                }



                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public List<Paquetes> Muestra_Paquetes()//muestra todos los registros activos
        {
            List<Paquetes> List = new List<Paquetes>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base};convert zero datetime=True";
            TareasACommands oTareas = new TareasACommands();
            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_paquetes_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();


                while (leer.Read())
                {
                    List.Add(new Paquetes()//llena la lista de datos
                    {
                        Guia = leer["paq_guia"].ToString(),
                        EstatusPaqueteDescripcion = leer["epa_descripcion"].ToString(),
                        Intento = Convert.ToInt32(leer["paq_intento"]),
                        Fecha_Recepcion_String = leer.GetDateTime("paq_fecharecepcion").ToString(),
                        Paq_Descripcion = leer["gui_descripcion"].ToString(),
                        Paq_Instrucciones = leer["gui_instrucciones"].ToString(),
                        Destinatario = leer["Nombre de Destinatario"].ToString(),
                        Direccion = leer["Direccion"].ToString(),
                        Razon_Social_Cliente = leer["cli_razonsocial"].ToString()

                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public List<SeguimientoPaquetes> MuestraHistorialPaquetes()
        {
            List<SeguimientoPaquetes> list = new List<SeguimientoPaquetes>();

            string connectionsString = $"server = {GetRDSConections().Writer}; {Data_base};convert zero datetime=True";

            //conexión a la base de datos solo lectura
            using (MySqlConnection conexion = new MySqlConnection(connectionsString))
            {
                //Comandos generales
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_paquetes_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                //Ciclo que llena la lista con datos

                while (leer.Read())
                {
                    SeguimientoPaquetes seguimientoPaquetes = new SeguimientoPaquetes();

                    seguimientoPaquetes.ClienteID = Convert.ToInt32(leer["cli_id"]);
                    seguimientoPaquetes.Cliente = leer["cli_contacto"].ToString();
                    seguimientoPaquetes.PaqueteID = Convert.ToInt32(leer["paq_id"]);
                    seguimientoPaquetes.NombreDestinatario = leer["Nombre de Destinatario"].ToString();
                    seguimientoPaquetes.Guia = leer["paq_guia"].ToString();
                    seguimientoPaquetes.FechaRecepcion = Convert.ToDateTime(leer["paq_fecharecepcion"].ToString());
                    seguimientoPaquetes.Estatus = leer["epa_descripcion"].ToString();
                    seguimientoPaquetes.fechaRecepcion = (leer["paq_fecharecepcion"].ToString());
                    seguimientoPaquetes.fechaEntrega = leer["paq_fechaentregafinal"].ToString();
                    seguimientoPaquetes.EvidenciaEntrega = leer["tar_evidencia"].ToString();
                    seguimientoPaquetes.RazonSocial = leer["cli_razonsocial"].ToString();
                    seguimientoPaquetes.PalabraClave = leer["paq_codigorecoleccion"].ToString();
                    seguimientoPaquetes.Direccion = leer["Direccion"].ToString();
                    if (leer["paq_fechaentregafinal"].ToString() == "")
                    {
                        seguimientoPaquetes.FechaEntrega = null;
                    }
                    else
                    {
                        seguimientoPaquetes.FechaEntrega = Convert.ToDateTime(leer["paq_fechaentregafinal"].ToString());
                    }

                    list.Add(seguimientoPaquetes);
                }

                //Cierre general
                conexion.Close();//Cierra conexión
                leer.Close();//Cierra lista
            }

            return list;//Devuelve la lista con datos
        }

        public SeguimientoPaquetes MuestraPaquetesMod(int id)
        {
            SeguimientoPaquetes list = new SeguimientoPaquetes();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            //Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_paquetesmod_sp";
                cmd.Parameters.AddWithValue("paqid", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                //Llena lista de datos
                while (leer.Read())
                {
                    list.ClienteID = leer.GetInt32("cli_id");
                    list.Guia = leer["gui_guia"].ToString();
                    list.NombreDestinatario = leer["Nombre de Destinatario"].ToString();
                    list.FechaRecepcion = Convert.ToDateTime(leer["paq_fecharecepcion"].ToString());
                    list.Estatus = leer["epa_descripcion"].ToString();
                    list.EvidenciaEntrega = leer["tar_evidencia"].ToString();
                    list.Direccion = leer["Direccion"].ToString();
                    list.Condicion = leer["con_descripcion"].ToString();
                    list.PalabraClave = leer["paq_codigorecoleccion"].ToString();
                    list.Nintentos = leer["paq_intento"].ToString();
                }

                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return list;//regresa la lista con datos
            }
        }

        public List<Paquetes> Muestra_PaquetesUno()//muestra todos los registros activos
        {
            List<Paquetes> List = new List<Paquetes>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base};convert zero datetime=True";
            TareasACommands oTareas = new TareasACommands();
            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_paquetes_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Paquetes()//llena la lista de datos
                    {
                        Guia = leer["paq_guia"].ToString(),
                        Intento = leer.GetInt32("paq_intento"),
                        Zona = leer["zon_descripcion"].ToString(),
                        FechaIngresoBodega = leer["paq_fechaingresobodega"].ToString(),
                        FechaEntregaFinal = leer["paq_fechaentregafinal"].ToString(),
                        EstatusPaqueteDescripcion = leer["epa_descripcion"].ToString()
                    });
                }
                // comparamos la lista de tareas con la lista de paquetes para solo mostrar
                // los paquetes que no se muestren en la lista de tareas


                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }





    }
}