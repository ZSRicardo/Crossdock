using Crossdock.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Crossdock.Context.Commands
{
    public class TablaUsuariosCommands : CrossDockContext
    {
        /// <summary>
        /// Da de alta un nuevo registro Usuario en la base de datos. Genera un nuevo registro si se ingresa el id en "0", si no, modifica el registro existente.
        /// </summary>
        public void Alta_Usuario(Usuario User)
        {
            //Conexión a la base de datos //Writer porque Altas son escrituras
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Alta
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "alta_usuarios_sp";
                

                /*Pasar las propiedades a los parametros del SP*/
                /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                /*cmd.Parameters.AddWithValue("(nombre del parametro en el SP)", OBJECT.PARAMETER);*/

                cmd.Parameters.AddWithValue("usuid", User.UsuarioID);
                cmd.Parameters.AddWithValue("usunombre", User.Nombre);
                cmd.Parameters.AddWithValue("usupassword", User.Password);
                cmd.Parameters.AddWithValue("usufechaalta", User.UsuFechaAlta);
                //Mandar a null. Es DateTime?, se manda null.
                cmd.Parameters.AddWithValue("usu_fechabaja", null);
                //Mandar a null. Es String, se manda cadena vacía.
                cmd.Parameters.AddWithValue("usucodigoverificacion", "");
                //Mandar a null. Es Bool, se manda 0.
                cmd.Parameters.AddWithValue("usucelularverificado", 0);
                cmd.Parameters.AddWithValue("usuemail", User.Email);
                //Mandar a null. Es Bool, se manda 0.
                cmd.Parameters.AddWithValue("usuemailverificado", 0);
                cmd.Parameters.AddWithValue("usucelular", User.Celular);
                cmd.Parameters.AddWithValue("rolid", User.RolID);
                cmd.Parameters.AddWithValue("cliid", User.ClienteID);

                string caracteres = User.Nombre.Substring(0, 3).ToUpper();//extrae los 3 primeros digitos del nombre
                var prueba = Convert.ToString(DateTime.Now.DayOfYear);//dia juliano
                var ano = DateTime.Now.Year.ToString().Remove(0, 2);//año actual
                var identificador = prueba + ano + caracteres;

                cmd.Parameters.AddWithValue("usuidentificador", User.UsuarioIdentificador = identificador);

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();
                cmd = null;
            }
        }

        /// <summary>
        /// Muestra todos los registros Usuario activos.
        /// </summary>
        public List<Usuario> Muestra_Usuario()
        {
            List<Usuario> List = new List<Usuario>();
            //Conexión a la base de datos //Reader porque es solo lectura
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Muestra
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "muestra_usuarios_sp"
                };
                conexion.Open();
                var leer = cmd.ExecuteReader();

                //Ciclo que llena la lista de datos
                while (leer.Read())
                {
                    List.Add(new Usuario()
                    {
                        /*Igualar las propiedades con los parametros del SP*/
                        /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                        UsuarioID=leer.GetInt32("usu_id"),
                        UsuarioIdentificador = leer.GetString("usu_identificador"),
                        Nombre =leer.GetString("usu_nombre"),
                        Password=leer["usu_password"].ToString(),
                        Email=leer["usu_email"].ToString(),
                        Celular=leer["usu_celular"].ToString(),
                        RolID =Convert.ToInt32(leer["rol_id"].ToString()),
                        ClienteID = Convert.ToInt32(leer["cli_id"].ToString()),


                    });
                }

                // Cierre General
                conexion.Close();//Cierra conexion
                leer.Close();//Cierra lista
                return List;//Devuelve la lista con datos
            }
        }

        /// <summary>
        /// Muestra un registro Usuario con base en un id.
        /// </summary>
        public List<Usuario> Muestra_UsuarioMod(int id)
        {
            List<Usuario> List = new List<Usuario>();
            //Conexión a la base de datos //Reader porque es solo lectura
            string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales MuestraMod
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "muestra_usuariosmod_sp"
                };

                /*Igualar las propiedades con los parametros del SP*/
                /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                cmd.Parameters.AddWithValue("usuid", id);
                conexion.Open();
                var leer = cmd.ExecuteReader();

                //Ciclo que llena la lista de datos
                while (leer.Read())
                {
                    List.Add(new Usuario()
                    {
                        /*Igualar las propiedades con los parametros del SP*/
                        /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                        //Del Usuario
                        UsuarioID = leer.GetInt32("usu_id"),
                        UsuarioIdentificador = leer.GetString("usu_identificador"),
                        Nombre =leer["usu_nombre"].ToString(),
                        UsuFechaAlta=Convert.ToDateTime(leer["usu_fechaalta"].ToString()),
                        Celular=leer["usu_celular"].ToString(),
                        Email=leer["usu_email"].ToString(),
                        RolID = Convert.ToInt32(leer["rol_id"].ToString()),
                        ClienteID = Convert.ToInt32(leer["cli_id"].ToString()),
                        //De Roles
                        Descripcion =leer["rol_descripcion"].ToString(),
                        Permisos=leer["rol_permisos"].ToString(),

                        //De Clientes
                        Cliente=(leer["cli_razonsocial"].ToString()+" "+leer["cli_contacto"].ToString()+" "+leer["cli_telefono"].ToString()+" "+leer["cli_email"].ToString()),

                        ClienteDireccion=(leer["cli_calle"].ToString()+" "+leer["cli_numeroext"].ToString()+" "+leer["cli_numeroint"].ToString()+" "+leer["cli_colonia"].ToString()+" "+leer["cli_codigopostal"].ToString()),

                    });
                }

                // Cierre General
                conexion.Close();//Cierra conexion
                leer.Close();//Cierra lista
                return List;//Devuelve la lista con datos
            }
        }

        /// <summary>
        /// Borrado logico de un registro Usuario en la base de datos.
        /// </summary>
        public void Elimina_Usuario(int id)
        {
            string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                //Comandos Generales Elimina
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conexion,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "elimina_usuarios_sp"
                };

                // Parametros de SP
                cmd.Parameters.AddWithValue("usuid", id);

                // Cierre General
                conexion.Open();
                int res = cmd.ExecuteNonQuery();
                conexion.Close();//Cierra conexion
                cmd = null;
            }
        }
    }
}