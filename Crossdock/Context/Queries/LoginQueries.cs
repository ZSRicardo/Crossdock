using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Crossdock.Context.Queries
{
    public class LoginQueries : CrossDockContext
    {

        // Valida acceso y retorna celular
        public Login Login(LoginViewModel loginViewModel)
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
                cmd.CommandText = "ValidaAcceso_Usuario_sp";

                // Parametros de SP
                cmd.Parameters.AddWithValue("usuemail", loginViewModel.Email);
                cmd.Parameters.AddWithValue("usupassword", loginViewModel.Password);

                // Lectura de datos
                using (var reader = cmd.ExecuteReader())
                {
                    // Respuesta
                    var login = new Login();

                    while (reader.Read())
                    {
                        login.UsuarioID = Convert.ToInt32(reader["usu_id"]);
                        login.UsuarioNombre = reader["usu_nombre"].ToString();
                        login.ClienteID = Convert.ToInt32(reader["cli_id"]);
                        login.Permisos = reader["rol_permisos"].ToString();
                        login.UsuarioIdendtificador = reader["usu_identificador"].ToString();
                        login.UsuarioRolID = Convert.ToInt32(reader["rol_id"]);
                        login.ClienteRS = reader["cli_razonsocial"].ToString();
                        login.ClienteIdentifiador = reader["cli_identificador"].ToString();

                    }

                    return login;
                }
            }
        }

    } // Fin de Clase
}