using Crossdock.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace Crossdock.Context.Queries
{
    public class TablaClientesQueries : CrossDockContext
    {
        // Muestra datos de un cliente, falta agregar campos restantes de fila
        public Clientes Muestra_ClientesMod(int clienteID)
        {

            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";

            // Utiliza dispose al finalizar bloque
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                // Comandos
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "muestra_clientesmod_sp";
                // Parametros de SP

                cmd.Parameters.AddWithValue("cl_id", clienteID);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                Clientes cliente = new Clientes();

                while (leer.Read())
                {

                    cliente.RazonSocial = leer["cli_razonsocial"].ToString();
                }
                conexion.Close();//cierra conexion
                leer.Close();//cierra lista
                return cliente;//regresa la lista con datos
            }
        }
    }
}