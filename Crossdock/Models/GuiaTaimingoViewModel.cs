using System.ComponentModel.DataAnnotations;

namespace Crossdock.Models
{
    public class GuiaTaimingoViewModel
    {
        //guias
        [Required(ErrorMessage = "Por favor seleccione el tipo de servicio")]
        [Display(Name = "Tipo de servicio:")]
        public bool T_Etiqueta { get; set; }


        [Required(ErrorMessage = "Por favor ingresa el nombre")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Maximo 50 caracteres")]
        [Display(Name = "Nombre(s):")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Por favor ingresa el apellido paterno")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Maximo 50 caracteres")]
        [Display(Name = "Apellido Paterno:")]
        public string ApellidoPaterno { get; set; }

        [StringLength(50, MinimumLength = 0, ErrorMessage = "Maximo 50 caracteres")]
        [Display(Name = "Apellido Materno:")]
        public string ApellidoMaterno { get; set; }

        [StringLength(10, MinimumLength = 1, ErrorMessage = "Maximo 10 caracteres")]
        [Required(ErrorMessage = "Por favor ingresa el celular")]
        [Display(Name = "Celular:")]
        public string Celular { get; set; }

        // [Required(ErrorMessage = "Por favor ingresa el correo electronico")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Maximo 50 caracteres")]
        [EmailAddress(ErrorMessage = "Formato invalido")] // Validar el tipo de input email
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Por favor ingresa  la calle")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Maximo 50 caracteres")]
        [Display(Name = "Calle:")]
        public string Calle { get; set; }

        [Required(ErrorMessage = "Por favor ingresa el numero exterior")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Maximo 30 caracteres")]
        [Display(Name = "Numero Exterior:")]
        public string NumeroExt { get; set; }

        [StringLength(30, MinimumLength = 0, ErrorMessage = "Maximo 30 caracteres")]
        [Display(Name = "Numero Interior:")]
        public string NumeroInt { get; set; }

        [Required(ErrorMessage = "Por favor ingresa la colonia")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Maximo 50 caracteres")]
        [Display(Name = "Colonia:")]
        public string Colonia { get; set; }

        [Required(ErrorMessage = "Por favor ingresa codigo postal")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Requeridos 5 caracteres")]
        [Display(Name = "Codigo Postal:")]
        public string CodigoPostal { get; set; }

        [Required(ErrorMessage = "Por favor, selecciona una opción")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Maximo 20 caracteres")]
        [Display(Name = "Medida:")]
        public string Medida { get; set; }

        [Required(ErrorMessage = "Por favor, selecciona un rango de peso")]
        [Range(0, 200, ErrorMessage = "Rango invalido")]
        [Display(Name = "Peso:")]
        public double Peso { get; set; }

        //   [Required(ErrorMessage = "Por favor ingresa una descripcion")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Maximo 100 caracteres")]
        [Display(Name = "Descripción:")]
        // Listado de Categorias
        public string Descripcion { get; set; }

        //  [Required(ErrorMessage = "Por favor ingresa instrucciones de entrega")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Maximo 100 caracteres")]
        [Display(Name = "Instrucciones:")]
        public string Instrucciones { get; set; }

        //[Required(ErrorMessage = "Por favor selecciona una opción")]
        //[Display(Name = "Delivery/Empresa para entrega")]
        //// Delivery
        //public int? Del_id { get; set; }

        public string RazonSocialCliente { get; set; }
        //agregamos los id para las llaves foraneas
        public int Usu_id { get; set; }
        public int Cli_id { get; set; }
        public int Des_id { get; set; }

        // ingresamos latitud y longitud de la direccion del destinatario 

        [Display(Name = "Latitud:")]
        public float? Des_Latitud { get; set; }
        [Display(Name = "Longitud:")]
        public float? Des_Longitud { get; set; }
        [Display(Name = "Numero de Pedido:")]
        public string NoPedido { get; set; }
        public string Guia { get; set; }

        public int TServicio { get; set; }

        [Display(Name = "Total de paquetes:")]
        public int Numero_de_Paquetes_Extra { get; set; }


    }
}