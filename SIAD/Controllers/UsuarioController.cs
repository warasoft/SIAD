using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIAD.Models;
using SIAD.Servicios;
using SIAD.Entidades;
using Microsoft.DotNet.Scaffolding.Shared.Project;

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

        [HttpPost]
        public async Task<IActionResult> AltaUsuario(UsuarioListViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = new Usuario()
            {
                Matricula = modelo.UsuarioVM.Matricula,
                Grado = modelo.UsuarioVM.Grado,
                Apellido = modelo.UsuarioVM.Apellido,
                Nombre = modelo.UsuarioVM.Nombre,
                Destino = modelo.UsuarioVM.Destino,
                DeptoDiv = modelo.UsuarioVM.DeptoDiv,
                Email = modelo.UsuarioVM.Email,
                PhoneNumber = modelo.UsuarioVM.PhoneNumber,
                UserName = modelo.UsuarioVM.UserName
            };

            var usuario = await userManager.FindByIdAsync();

            if (usuario == null)
            {
                return NotFound();
            }

            userManager.Options.Password.RequiredLength = 4; // Establece la longitud mínima requerida para la contraseña (puedes ajustarlo según tus necesidades)
            userManager.Options.Password.RequireDigit = false;
            userManager.Options.Password.RequireLowercase = false;
            userManager.Options.Password.RequireUppercase = false;
            userManager.Options.Password.RequireNonAlphanumeric = false;

            var resultado = await userManager.CreateAsync(usuario, modelo.UsuarioVM.PasswordHash);

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

        [HttpPost]
        public async Task<IActionResult> ModificarUsuario(UsuarioListViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = await userManager.FindByIdAsync(modelo.UsuarioVM.Id);

            if (usuario != null)
            {
                // Actualizar las propiedades del usuario con los datos del modelo
                usuario.UserName = modelo.UsuarioVM.UserName;
                usuario.Destino = modelo.UsuarioVM.Destino;
                usuario.Grado = modelo.UsuarioVM.Grado;
                usuario.Matricula = modelo.UsuarioVM.Matricula;
                usuario.Apellido = modelo.UsuarioVM.Apellido;
                usuario.DeptoDiv = modelo.UsuarioVM.DeptoDiv;
                usuario.Email = modelo.UsuarioVM.Email;
                usuario.PhoneNumber = modelo.UsuarioVM.PhoneNumber;

                // Actualizar el usuario en la base de datos utilizando userManager
                var resultado = await userManager.UpdateAsync(usuario);

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
            var modelo = new UsuarioListViewModel();
            modelo.ListaUsuariosVM = new ListaUsuarioViewModel(); // Inicializar la instancia de ListaUsuarios
            modelo.ListaUsuariosVM.Usuarios = usuarios;
            modelo.ListaUsuariosVM.Mensaje = mensaje;
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
    }
}
