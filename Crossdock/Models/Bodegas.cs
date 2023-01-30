using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossdock.Models
{
    public class Bodegas
    {
        //Bodegas_tb
        [Key]
        [Column(Order = 0)]
        public int BodegaID { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [RegularExpression(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"+ @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"  + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", ErrorMessage = "Email Inválido")]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(\d{7,10}|\d{10,10})$", ErrorMessage = "Teléfono Inválido")]
        [Display(Name = "Celular")]
        public long Telefono { get; set; }

        [Required]
        [Display(Name = "Calle")]
        public string Calle { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese un valor numérico")]
        [Display(Name = "Número exterior")]
        public string NumeroExt { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Ingrese un valor numérico")]
        [Display(Name = "Número interior")]
        public string NumeroInt { get; set; }

        [Required]
        [Display(Name = "Colonia")]
        public string Colonia { get; set; }

        [Required]
        [RegularExpression(@"^\d{4,5}$", ErrorMessage = "Código Postal Inválido. Ingresa 5 dígitos.")]
        [Display(Name = "Código postal")]
        public string CodigoPostal { get; set; }

        [Required]
        [Display(Name = "Horario de inicio")]
        [RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (AM|PM)$", ErrorMessage = "Hora inválida. Use el formato de 12 horas: HH:MM AM/PM")]
        public string HorarioInicio { get; set; }

        [Display(Name = "Horario final")]
        [RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (AM|PM)$", ErrorMessage = "Hora inválida. Use el formato de 12 horas: HH:MM AM/PM")]
        public string HorarioFinal { get; set; }

        public int ID_Temporal { get; set; }
    }
}