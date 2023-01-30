using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class Operadores
    {
        //operadores
        [Key]
        [Column(Order = 0)]
        public int OperadorID { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoP { get; set; }

        [Required]
        [Display(Name = "Apellido Materno")]
        public string ApellidoM { get; set; }

        [Required]
        [RegularExpression(@"^(\d{7,10}|\d{10,10})$", ErrorMessage = "Celular Inválido")]
        [Display(Name = "Celular")]
        public string Celular { get; set; }

        [Required]
        [RegularExpression(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"+ @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"  + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", ErrorMessage = "Email Inválido")]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Calle")]
        public string Calle { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese un valor numérico")]
        [Display(Name = "Número Exterior")]
        public string NumeroExt { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Ingrese un valor numérico")]
        [Display(Name = "Número Interior")]
        public string NumeroInt { get; set; }

        [Required]
        [Display(Name = "Colonia")]
        public string Colonia { get; set; }

        [Required]
        [RegularExpression(@"^\d{4,5}$", ErrorMessage = "Código Postal Inválido. Ingresa 5 dígitos.")]
        [Display(Name = "Código Postal")]
        public string CodigoPostal { get; set; }

        [Display(Name = "Tipo de Operador")]
        public int TipoOperadorID { get; set; }
        
        [Display(Name = "Descripción del Operador")]
        public string TipoOperadorDescripcion { get; set; }

        [Display(Name = "Unidad Asignada")]
        public int UnidadID { get; set; }
        
        [Display(Name = "Modelo de la Unidad")]
        public string UnidadModelo { get; set; }

        [Display(Name = "Marca de la Unidad")]
        public string UnidadMarca { get; set; }

        [Display(Name = "Placas de la Unidad")]
        public string UnidadPlacas { get; set; }


        public int ID_Temporal { get; set; }


    }
}