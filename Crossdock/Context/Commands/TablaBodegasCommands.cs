using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaBodegasCommands : CrossDockContext
    {
        /// <summary>
        /// Da de alta un nuevo objeto Bodegas en la base de datos. Genera un nuevo registro si se ingresa el id en "0", si no, modifica el registro existente.
        /// </summary>
        public void Alta_Bodegas(Bodegas bodegas)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_bodegas_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("bo_id", bodegas.BodegaID);
                cmd.Parameters.AddWithValue("bo_nombre", bodegas.Nombre);
                cmd.Parameters.AddWithValue("bo_email", bodegas.Email);
                cmd.Parameters.AddWithValue("bo_telefono", bodegas.Telefono);
                cmd.Parameters.AddWithValue("bo_calle", bodegas.Calle);
                cmd.Parameters.AddWithValue("bo_numeroext", bodegas.NumeroExt);
                cmd.Parameters.AddWithValue("bo_numeroint", bodegas.NumeroInt);
                cmd.Parameters.AddWithValue("bo_colonia", bodegas.Colonia);
                cmd.Parameters.AddWithValue("bo_codigopostal", bodegas.CodigoPostal);
                cmd.Parameters.AddWithValue("bo_horarioinicio", bodegas.HorarioInicio);
                cmd.Parameters.AddWithValue("bo_horariofinal", bodegas.HorarioFinal);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }

        /// <summary>
        /// Muestra todas las Bodegas activas.
        /// </summary>
        public List<Bodegas> Muestra_Bodegas()
        {
            List<Bodegas> List = new List<Bodegas>();
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_bodegas_sp";
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Bodegas()//llena la lista de datos
                    {
                        BodegaID = leer.GetInt32("bod_id"),
                        Nombre=leer["bod_nombre"].ToString(),
                        Email=leer["bod_email"].ToString(),
                        Telefono=Convert.ToInt64(leer["bod_telefono"].ToString()),
                        Calle =leer["bod_calle"].ToString(),
                        NumeroExt=leer["bod_numeroext"].ToString(),
                        NumeroInt=leer["bod_numeroint"].ToString(),
                        Colonia=leer["bod_colonia"].ToString(),
                        CodigoPostal=leer["bod_codigopostal"].ToString(),
                        HorarioInicio=leer["bod_horarioinicio"].ToString(),
                        HorarioFinal=leer["bod_horariofinal"].ToString(),
                    });
                }

                conexion.Close();
                leer.Close();
                return List;
            }
        }

        public List<Bodegas> Muestra_BodegasMod(int id)//muestra una bodega basado en  un id
        {
            List<Bodegas> List = new List<Bodegas>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_bodegasmod_sp";
                cmd.Parameters.AddWithValue("bo_id", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Bodegas()//llena la lista de datos
                    {
                        BodegaID = leer.GetInt32("bod_id"),
                        Nombre=leer["bod_nombre"].ToString(),
                        Email=leer["bod_email"].ToString(),
                        Telefono=Convert.ToInt64(leer["bod_telefono"].ToString()),
                        Calle =leer["bod_calle"].ToString(),
                        NumeroExt=leer["bod_numeroext"].ToString(),
                        NumeroInt=leer["bod_numeroint"].ToString(),
                        Colonia=leer["bod_colonia"].ToString(),
                        CodigoPostal=leer["bod_codigopostal"].ToString(),
                        HorarioInicio=leer["bod_horarioinicio"].ToString(),
                        HorarioFinal=leer["bod_horariofinal"].ToString(),
                    });
                }

                //Cierre General
                conexion.Close();//Cierra conexión
                leer.Close();//Cierra lista
                return List;// Devuelve la lista con datos
            }
        }

        public void Elimina_Bodegas(int id)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "elimina_bodegas_sp";
                cmd.Parameters.AddWithValue("bo_id", id);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }
    }
}