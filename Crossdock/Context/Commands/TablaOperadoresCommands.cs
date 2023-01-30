using Crossdock.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaOperadoresCommands : CrossDockContext
    {
        public void Alta_Operadores(Operadores op)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "alta_operadores_sp"
                };

                // Parametros de SP

                cmd.Parameters.AddWithValue("op_id", op.OperadorID);
                cmd.Parameters.AddWithValue("op_nombre", op.Nombre);
                cmd.Parameters.AddWithValue("op_apellidop", op.ApellidoP);
                cmd.Parameters.AddWithValue("op_apellidom", op.ApellidoM);
                cmd.Parameters.AddWithValue("op_celular", op.Celular);
                cmd.Parameters.AddWithValue("op_email", op.Email);
                cmd.Parameters.AddWithValue("op_calle", op.Calle);
                cmd.Parameters.AddWithValue("op_numeroext", op.NumeroExt);
                cmd.Parameters.AddWithValue("op_numeroint", op.NumeroInt);
                cmd.Parameters.AddWithValue("op_colonia", op.Colonia);
                cmd.Parameters.AddWithValue("op_codigopostal", op.CodigoPostal);
                cmd.Parameters.AddWithValue("to_id", op.TipoOperadorID);
                cmd.Parameters.AddWithValue("un_id", op.UnidadID);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }//Genera un nuevo registro si se ingresa el id en 0 si no modifica los datos

        public List<Operadores> Muestra_Operadores()//muestra todos los comdiciones activos
        {
            List<Operadores> List = new List<Operadores>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "muestra_operadores_sp"
                };
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Operadores()//llena la lista de datos
                    {
                        OperadorID = leer.GetInt32("ope_id"),
                        Nombre  = leer["ope_nombre"].ToString(),
                        ApellidoP = leer["ope_apellidop"].ToString(),
                        ApellidoM= leer["ope_apellidom"].ToString(),
                        Celular= leer["ope_celular"].ToString(),
                        Email= leer["ope_email"].ToString(),
                        CodigoPostal = leer["ope_codigopostal"].ToString(),
                    });
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public List<Operadores> Muestra_OperadoresMod(int op_id)//muestra una condiciones basados en el id
        {
            List<Operadores> List = new List<Operadores>();
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "muestra_operadoresmod_sp"
                };
                // Parametros de SP

                cmd.Parameters.AddWithValue("op_id", op_id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                while (leer.Read())
                {
                    List.Add(new Operadores()
                    {
                        OperadorID = leer.GetInt32("ope_id"),
                        Nombre = leer["ope_nombre"].ToString(),
                        ApellidoP = leer["ope_apellidop"].ToString(),
                        ApellidoM = leer["ope_apellidom"].ToString(),
                        Celular = leer["ope_celular"].ToString(),
                        Email = leer["ope_email"].ToString(),
                        Calle = leer["ope_calle"].ToString(),
                        NumeroExt = leer["ope_numeroext"].ToString(),
                        NumeroInt = leer["ope_numeroint"].ToString(),
                        Colonia = leer["ope_colonia"].ToString(),
                        CodigoPostal = leer["ope_codigopostal"].ToString(),
                        TipoOperadorID = leer.GetInt32("top_id"),
                        UnidadID = leer.GetInt32("uni_id"),
                    });

                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return List;//regresa la lista con datos
            }
        }

        public void Elimina_Operadores(int op_id)
        {

            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "elimina_operadores_sp"
                };

                // Parametros de SP

                cmd.Parameters.AddWithValue("op_id", op_id);

                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }//borrado logico de un registro

    }
}