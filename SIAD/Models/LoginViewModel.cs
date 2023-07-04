using System.ComponentModel.DataAnnotations;

namespace SIAD.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]

        public string Usuario { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Password { get; set; }

        [Display(Name = "Recordar")]
        public bool Recordar { get; set; }
    }
}
