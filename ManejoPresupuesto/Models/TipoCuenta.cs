using ManejoPresupuesto.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")] //{0} Para que aparezca "Nombre" en el mensaje
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "La longitud del campo {0} debe estar entre {2} y {1}")] //Para numero de caracteres
        [Display(Name = "Nombre del tipo cuenta")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }


        //Pruebas de otras validaciones por defecto Data Anootations
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo valido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(minimum: 18, maximum: 70, ErrorMessage = "El valor debe estar entre {1} y {2} años")]
        public int Edad { get; set; }
        [Url]
        public string URL { get; set; }
        [CreditCard]
        public string TarjetaCredito { get; set; }

    }
}
