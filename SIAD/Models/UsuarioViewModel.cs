using System.ComponentModel.DataAnnotations;

namespace SIAD.Models
{
    public class UsuarioViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int NumeroUsuario { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo {0} debe contener solo números")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Matricula { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Grado { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Apellido { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Destino { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string DeptoDiv { get; set; }

        [EmailAddress(ErrorMessage = "El campo {0} debe ser un correo electrónico válido")]
        public string Email { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo {0} debe contener solo números")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Interno { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Password { get; set; }

    }
}
