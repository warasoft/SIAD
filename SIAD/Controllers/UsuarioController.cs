using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIAD.Models;
using SIAD.Servicios;
using SIAD.Entidades;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace SIAD.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> userManager;
        private readonly SignInManager<Usuario> signInManager;
        private readonly ApplicationDbContext context;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Perfil()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult AltaUsuario()
        {
            ViewBag.NomUs = GenerarNomUsuario();
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AltaUsuario(UsuarioViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NomUs = GenerarNomUsuario();
                return View(modelo);
            }


            int maxNumeroUsuario = ObtenerMaxNumeroUsuario();


            var usuario = new Usuario()
            {
                Matricula = modelo.Matricula,
                Grado = modelo.Grado,
                Apellido = modelo.Apellido,
                Nombre = modelo.Nombre,
                Destino = modelo.Destino,
                DeptoDiv = modelo.DeptoDiv,
                Email = modelo.Email,
                PhoneNumber = modelo.Interno,
                UserName = modelo.NombreUsuario,
                NumeroUsuario= maxNumeroUsuario +1
            };

            if (usuario == null)
            {
                return NotFound();
            }

            userManager.Options.Password.RequiredLength = 4; // Establece la longitud mínima requerida para la contraseña (puedes ajustarlo según tus necesidades)
            userManager.Options.Password.RequireDigit = false;
            userManager.Options.Password.RequireLowercase = false;
            userManager.Options.Password.RequireUppercase = false;
            userManager.Options.Password.RequireNonAlphanumeric = false;

            var resultado = await userManager.CreateAsync(usuario, modelo.Password);

            if (resultado.Succeeded)
            {
                await context.SaveChangesAsync();
                return RedirectToAction("Listado", new { mensaje = "Usuario creado exitosamente" });
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(modelo);
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ModificarUsuario(string id)
        {
            var usuario = await userManager.FindByIdAsync(id);

            if (usuario is null)
            {
                return NotFound();
            }

            var usuarioViewModel = new UsuarioViewModel
            {
                NombreUsuario = usuario.UserName,
                Matricula = usuario.Matricula,
                Grado = usuario.Grado,
                Apellido = usuario.Apellido,
                Nombre = usuario.Nombre,
                Destino = usuario.Destino,
                DeptoDiv = usuario.DeptoDiv,
                Interno = usuario.PhoneNumber,
                Email = usuario.Email,
                Password = usuario.PasswordHash
            };

            return View(usuarioViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ModificarUsuario(UsuarioViewModel modelo)
        {

            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = await userManager.FindByNameAsync(modelo.NombreUsuario);

            if (modelo.Password == "0000")
            {
                modelo.Password = usuario.PasswordHash;
            }

            

            if (usuario != null)
            {
                usuario.Matricula = modelo.Matricula;
                usuario.Grado = modelo.Grado;
                usuario.Apellido = modelo.Apellido;
                usuario.Nombre = modelo.Nombre;
                usuario.Destino = modelo.Destino;
                usuario.DeptoDiv = modelo.DeptoDiv;
                usuario.Email = modelo.Email;
                usuario.PhoneNumber = modelo.Interno;

                
                userManager.Options.Password.RequiredLength = 4; // Establece la longitud mínima requerida para la contraseña (puedes ajustarlo según tus necesidades)
                userManager.Options.Password.RequireDigit = false;
                userManager.Options.Password.RequireLowercase = false;
                userManager.Options.Password.RequireUppercase = false;
                userManager.Options.Password.RequireNonAlphanumeric = false;





                var resultado = await userManager.UpdateAsync(usuario);

                //var resultado = await userManager.UpdateAsync(usuario);


                if (resultado.Succeeded)
                {
                    // El usuario se actualizó exitosamente, ahora guarda los cambios en la base de datos
                    await context.SaveChangesAsync();

                    // Resto del código después de guardar los cambios...
                    return RedirectToAction("Listado", new { mensaje = "Usuario modificado exitosamente" });
                }
                else
                {
                    // El usuario no se pudo actualizar, maneja el resultado de manera adecuada
                    // y muestra mensajes de error si es necesario
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                // El usuario no se encontró, muestra un mensaje de error
                ModelState.AddModelError("", "El usuario no existe");
            }

            return View(modelo);
        }

        //public string CambioPassword(UsuarioViewModel modelo)
        //{
        //    if (modelo == null)
        //    {

        //    }
        //    var nuevoPass = userManager.ChangePasswordAsync(Usuario, Usuario.password, modelo.Password);
        //}

        [Authorize]
        public async Task<ActionResult> ExisteUserName(string userName)
        {
            var usuario = await userManager.FindByIdAsync(userName);

            if (usuario == null)
            {
                return NotFound();
            }

            // Realiza acciones adicionales con el usuario, si es necesario

            return Ok(usuario);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ObtenerUsuario(string id)
        {
            var usuario = await userManager.FindByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            // Realiza acciones adicionales con el usuario, si es necesario

            return Ok(usuario);
        }

        [HttpGet]
        [Authorize]
        public int ObtenerMaxNumeroUsuario()
        {
            var maxNumeroUsuario = context.Users.Max(u => u.NumeroUsuario);

            if (maxNumeroUsuario == 0)
            {
                return 0;
            }

            return maxNumeroUsuario;
        }

        [HttpGet]
        [Authorize]
        public string GenerarNomUsuario()
        {
            var numeroNuevo = ObtenerMaxNumeroUsuario() + 1;
            var NomUsuario = "SIAD" + numeroNuevo;
            return NomUsuario;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await signInManager.PasswordSignInAsync(modelo.Usuario, modelo.Password, modelo.Recordar, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Usuario o Contraseña Incorrecta!!!");
                return View(modelo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Login", "Usuario");
        }

        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Listado(string mensaje = null)
        {
            var usuarios = await userManager.Users.Select(u => new Usuario
            {
                Id = u.Id,
                UserName = u.UserName,
                Grado = u.Grado,
                Matricula = u.Matricula,
                Apellido = u.Apellido,
                Nombre = u.Nombre,
                Destino = u.Destino,
                DeptoDiv =  u.DeptoDiv,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber

            }).ToListAsync();
            var modelo = new ListaUsuarioViewModel();
            modelo = new ListaUsuarioViewModel(); // Inicializar la instancia de ListaUsuarios
            modelo.Usuarios = usuarios;
            modelo.Mensaje = mensaje;
            return View(modelo);
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> HacerAdmin(string user)
        {
            var usuario = context.Users.Where(u => u.UserName == user).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }

            await userManager.AddToRoleAsync(usuario, Constantes.RolAdmin);

            return RedirectToAction("Listado", routeValues: new { mensaje = "Rol asignado correctamente a " + user });
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> RemoverAdmin(string email)
        {
            var usuario = context.Users.Where(u => u.Email == email).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }

            await userManager.RemoveFromRoleAsync(usuario, Constantes.RolAdmin);

            return RedirectToAction("Listado", routeValues: new { mensaje = "Rol removido correctamente a " + email });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var nameUser = modelo.Nombre.Substring(0, 1) + modelo.Apellido;

            var usuario = new Usuario() { UserName = nameUser };

            userManager.Options.Password.RequiredLength = 4; // Establece la longitud mínima requerida para la contraseña (puedes ajustarlo según tus necesidades)
            userManager.Options.Password.RequireDigit = false;
            userManager.Options.Password.RequireLowercase = false;
            userManager.Options.Password.RequireUppercase = false;
            userManager.Options.Password.RequireNonAlphanumeric = false;

            var resultado = await userManager.CreateAsync(usuario, modelo.Password);



            //var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);

            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(modelo);
            }
        }
    }
}
