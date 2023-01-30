
/// <summary>
/// Da de alta un nuevo registro XXXX en la base de datos. Genera un nuevo registro si se ingresa el id en "0", si no, modifica el registro existente.
/// </summary>
public void Alta_XXXX(XXXX XXXX_OBJECT)
{
    //Conexión a la base de datos //Writer porque Altas son escrituras
    string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
    using (MySqlConnection conexion = new MySqlConnection(connectionString))
    {   
        //Comandos Generales Alta
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conexion;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "(nombre del procedimiento almacenado de alta)";

        /*Pasar las propiedades a los parametros del SP*/
        /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
        cmd.Parameters.AddWithValue("(nombre del parametro en el SP)", OBJECT.PARAMETER);

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
        cmd.Parameters.AddWithValue("bo_horariofinal",bodegas.HorarioFinal);

        // Cierre General
        conexion.Open();
        int res = cmd.ExecuteNonQuery();
        conexion.Close();
        cmd = null;
    }
}


/// <summary>
/// Muestra todos los registros XXXX activos.
/// </summary>
public List<Bodegas> Muestra_XXXX()
{
    List<XXXX> List = new List<XXXX>();
    //Conexión a la base de datos //Reader porque es solo lectura
    string connectionString = $"server ={GetRDSConections().Reader}; {Data_base}";
    using (MySqlConnection conexion = new MySqlConnection(connectionString))
    {
        //Comandos Generales Muestra
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conexion;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "(nombre del procedimiento almacenado que muestra)";
        conexion.Open();
        var leer = cmd.ExecuteReader();

        //Ciclo que llena la lista de datos
        while (leer.Read())
        {
            List.Add(new XXXX()
            {
                /*Igualar las propiedades con los parametros del SP*/
                /*Lo siguiente sirve de acordeon. Debe modificarse para cada SP.*/
                Nombre=leer["bod_nombre"].ToString(),
                Email=leer["bod_email"].ToString(),
                Telefono=Convert.ToInt64(leer["bod_telefono"].ToString()),
                Calle =leer["bod_calle"].ToString(),
                NumeroExt=leer["bod_numeroext"].ToString(),
                NumeroInt=leer["bod_numeroint"].ToString(),
                Colonia=leer["bod_colonia"].ToString(),
                CodigoPostal=leer["bod_codigopostal"].ToString(),
                HorarioInicio=Convert.ToDateTime(leer["bod_horarioinicio"].ToString()),
                HorarioFinal= Convert.ToDateTime(leer["bod_horariofinal"].ToString()),
            });
        }

        // Cierre General
        conexion.Close();//Cierra conexion
        leer.Close();//Cierra lista
        return List;//Devuelve la lista con datos
    }
}

/// <summary>
/// Muestra un registro XXXX con base en un id.
/// </summary>
public List<Bodegas> Muestra_BodegasMod(Bodegas bodegas)
{
    List<Bodegas> List = new List<Bodegas>();
    string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
    using (MySqlConnection conexion = new MySqlConnection(connectionString))
    {
        // Comandos
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conexion;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "muestra_bodegasmod_sp";
        // Parametros de SP

        cmd.Parameters.AddWithValue("bo_id", bodegas.BodegaID);
        conexion.Open();
        var leer = cmd.ExecuteReader();

        while (leer.Read())
        {
            List.Add(new Bodegas()//llena la lista de datos
            {

                Nombre = leer["bod_nombre"].ToString(),
                Email = leer["bod_email"].ToString(),
                Telefono = Convert.ToInt64(leer["bod_telefono"].ToString()),
                Calle = leer["bod_calle"].ToString(),
                NumeroExt = leer["bod_numeroext"].ToString(),
                NumeroInt = leer["bod_numeroint"].ToString(),
                Colonia = leer["bod_colonia"].ToString(),
                CodigoPostal = leer["bod_codigopostal"].ToString(),
                HorarioInicio = Convert.ToDateTime(leer["bod_horarioinicio"].ToString()),
                HorarioFinal = Convert.ToDateTime(leer["bod_horariofinal"].ToString()),



            });
        }
        conexion.Close();//cierra conexion
        leer.Close();//cierra lista
        return List;//regresa la lista con datos
    }
}

/// <summary>
/// Borrado logico de un registro XXXX en la base de datos.
/// </summary>
public void Elimina_Bodegas(Bodegas bodegas)
{
    string connectionString = $"server ={GetRDSConections().Writer}; {Data_base}";
    using (MySqlConnection conexion = new MySqlConnection(connectionString))
    {
        // Comandos
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conexion;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "elimina_bodegas_sp";

        // Parametros de SP
        cmd.Parameters.AddWithValue("bo_id", bodegas.BodegaID);
        conexion.Open();
        int res = cmd.ExecuteNonQuery();
        conexion.Close();
        cmd = null;
    }
}