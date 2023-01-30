using System;
using System.ComponentModel.DataAnnotations;

namespace Crossdock.Models
{
    public class Usuario
    {
        //Usuarios_tb
        public int UsuarioID { get; set; }

        [Display(Name = "Nombre del Usuario")]
        public string Nombre { get; set; }

        [Display(Name = "Contraseña de Usuario")]
        public string Password { get; set; }

        [Display(Name = "Fecha de la Alta")]
        public DateTime? UsuFechaAlta { get; set; }

        [Display(Name = "Fecha de la Baja")]
        public DateTime? UsuFechaBaja { get; set; }

        [Display(Name = "Codigo de Verificación")]
        public string CodigoVerificacion { get; set; }

        [Display(Name = "Verificación del Teléfono Registrado")]
        public bool TelefonoVerificado { get; set; }

        [Required]
        [RegularExpression(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"+ @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"  + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", ErrorMessage = "Email Inválido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Verificación del E-Mail Registrado")]
        public bool EmailVerificado { get; set; }

        [Required]
        [RegularExpression(@"^(\d{7,10}|\d{10,10})$", ErrorMessage = "Celular Inválido")]
        [Display(Name = "Celular")]
        public string Celular { get; set; }

        [Display(Name = "Rol Asignado")]
        public int RolID { get; set; }

        [Display(Name = "Cliente Asignado")]
        public int ClienteID { get; set; }

        [Display(Name = "Cliente Asignado")]
        public string Cliente { get; set; }

        [Display(Name = "Dirección del Cliente")]
        public string ClienteDireccion { get; set; }

        [Display(Name = "Descripción del Cliente")]
        public string Descripcion { get; set; }

        [Display(Name = "Rol Asignado")]
        public string Permisos { get; set; }

        [Display(Name = "Identificador de Usuario")]
        public string UsuarioIdentificador { get; set; }

        public string RolDescripcion { get; set; }
        public int ID_Temporal { get; set; }


    }
}