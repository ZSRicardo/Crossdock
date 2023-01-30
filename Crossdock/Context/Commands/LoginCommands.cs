using MySql.Data.MySqlClient;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class LoginCommands : CrossDockContext
    {

        // Metodo de ejemplo
        public bool Login(string parametro1, string parametro2)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "busqueda_Informacion_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("parametro1", parametro1);
                cmd.Parameters.AddWithValue("parametro2", parametro2);

                // Lectura de datos
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                    }
                }

                return true;
            }
        }

    } // Fin de clase
}