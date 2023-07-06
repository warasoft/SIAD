using Microsoft.AspNetCore.Identity;

namespace SIAD.Entidades
{
    public class Usuario : IdentityUser
    {
        public int NumeroUsuario { get; set; }        
        public int Matricula { get; set; }
        public string Grado { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Destino { get; set; }
        public string DeptoDiv { get; set;}
    }
}
