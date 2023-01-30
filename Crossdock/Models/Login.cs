namespace Crossdock.Models
{
    public class Login
    {
        //login
        public string UsuarioEmail { get; set; }

        public string UsuarioPassword { get; set; }

        public string UsuarioNombre { get; set; }

        public int UsuarioID { get; set; }

        public int ClienteID { get; set; }

        public string Permisos { get; set; }

        public string UsuarioIdendtificador { get; set; }

        public int RolID { get; set; }
        public int UsuarioRolID { get; set; }
        public string ClienteRS { get; set; }
        public string ClienteIdentifiador { get; set; }

    }
}