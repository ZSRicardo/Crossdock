using Crossdock.Models;
using DocumentFormat.OpenXml.Office.Word;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaEstatusTareasCommands : CrossDockContext
    {
        public void Alta_EstatusTareas(EstatusTareas estatus)
        {
            string connectionString = $"server = {GetRDSConections().Writer}; {Data_base}";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_estatus_tareas_sp";
                cmd.Parameters.AddWithValue("et_id", estatus.EstatusTareasID);
                cmd.Parameters.AddWithValue("et_descripcion", estatus.Descripcion);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }

        public List<EstatusTareas> Muestra_EstatusTareas()//muestra todos los registros activos
        {
            List<EstatusTareas> List = new List<EstatusTareas>();

            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_estatus_tareas_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new EstatusTareas()
                    {
                        EstatusTareasID = leer.GetInt32("eta_id"),
                        Descripcion = leer["eta_descripcion"].ToString(),
                   
                    });
                }

                conexion.Close();
                leer.Close();
                return List;
            }
        }



        public List<EstatusTareas> Muestra_EstatusTareasMod(int id)
        {
            List<EstatusTareas> List = new List<EstatusTareas>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_estatus_tareasmod_sp";

                cmd.Parameters.AddWithValue("et_id", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())

                {
                    List.Add(new EstatusTareas()
                    {
                        EstatusTareasID = leer.GetInt32("eta_id"),
                        Descripcion = leer["eta_descripcion"].ToString(),
                    });
                }

                conexion.Close();
                leer.Close();
                return List;
            }
        }

        public void Eliminar_EstatusTareas(int id)
        {
            string connectionString = $"server = {GetRDSConections().Writer}; {Data_base}";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_estatus_tareas_sp";
                cmd.Parameters.AddWithValue("et_id", id);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
            
        
        }

      



    }
}